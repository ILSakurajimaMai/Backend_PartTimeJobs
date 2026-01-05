import os
import json
import httpx
import logging  # <--- Thêm thư viện logging
from fastapi import FastAPI, Request
from fastapi.responses import JSONResponse, StreamingResponse

# --- CẤU HÌNH LOGGING ---
# Thiết lập định dạng log: [Giờ] [Mức độ] Nội dung
logging.basicConfig(
    level=logging.INFO,
    format="%(asctime)s [%(levelname)s] %(message)s",
    datefmt="%H:%M:%S"
)
logger = logging.getLogger("OpenRouterProxy")

# --- CẤU HÌNH SERVER ---
OPENROUTER_URL = "https://openrouter.ai/api/v1/chat/completions"
TARGET_MODEL = "your-model-id"

app = FastAPI(title="OpenAI Compatible Proxy with Logging")

async def forward_stream(response):
    """Generator để stream dữ liệu trả về"""
    try:
        async for chunk in response.aiter_bytes():
            yield chunk
    except Exception as e:
        logger.error(f"Error during streaming: {e}")

@app.post("/v1/chat/completions")
async def chat_completions(request: Request):
    
    # 1. Kiểm tra Auth
    auth_header = request.headers.get("Authorization")
    if not auth_header:
        logger.warning("Request missing Authorization header")
        return JSONResponse(status_code=401, content={"error": "Missing Authorization Header"})

    # 2. Đọc Body và LOG REQUEST
    try:
        body = await request.json()
        
        # --- ĐOẠN CODE GHI LOG ---
        # In ra nội dung request đẹp (pretty print)
        log_content = json.dumps(body, indent=2, ensure_ascii=False)
        logger.info(f"\n--- NEW REQUEST RECEIVED ---\n{log_content}\n----------------------------")
        # -------------------------

    except Exception:
        logger.error("Failed to parse JSON body")
        return JSONResponse(status_code=400, content={"error": "Invalid JSON body"})

    # 3. Sử dụng model từ request
    # Nếu client không gửi model, dùng model mặc định
    provided_model = body.get("model")
    if not provided_model:
        body["model"] = TARGET_MODEL
        logger.info(f"Model not specified. Using default: {TARGET_MODEL}")
    else:
        logger.info(f"Using requested model: {provided_model}")

    # 4. Chuẩn bị Header gửi sang OpenRouter
    headers = {
        "Authorization": auth_header,
        "Content-Type": "application/json",
        "HTTP-Referer": "http://localhost:8001",
        "X-Title": "Local Logging Proxy"
    }

    # 5. Gọi OpenRouter
    client = httpx.AsyncClient(timeout=60.0)
    try:
        req = client.build_request("POST", OPENROUTER_URL, headers=headers, json=body)
        response = await client.send(req, stream=True)

        if response.status_code != 200:
            error_content = await response.aread()
            await client.aclose()
            logger.error(f"OpenRouter Error {response.status_code}: {error_content.decode('utf-8')}")
            return JSONResponse(status_code=response.status_code, content=json.loads(error_content))

        if body.get("stream", False):
            logger.info("Streaming response started...")
            async def logged_stream():
                full_response = ""
                async for chunk in response.aiter_bytes():
                    yield chunk
                logger.info("Streaming response finished.")
            
            return StreamingResponse(logged_stream(), status_code=200, media_type="text/event-stream")
        
        else:
            content = await response.aread()
            await client.aclose()
            
            try:
                resp_json = json.loads(content)
                
                if "choices" in resp_json:
                    for choice in resp_json["choices"]:
                        if choice.get("finish_reason") is None:
                            choice["finish_reason"] = "stop"

                pretty_resp = json.dumps(resp_json, indent=2, ensure_ascii=False)
                logger.info(f"\n<<< RESPONSE RECEIVED <<<\n{pretty_resp}\n-------------------------")
                
                return JSONResponse(content=resp_json, status_code=200)
            except Exception as e:
                logger.error(f"Error processing response JSON: {e}")
                logger.info(f"<<< RESPONSE RECEIVED (Raw) <<<\n{content.decode('utf-8')}")
                return JSONResponse(content=json.loads(content), status_code=200)
            # ----------------------------------

    except Exception as e:
        await client.aclose()
        logger.critical(f"Internal Server Error: {str(e)}")
        return JSONResponse(status_code=500, content={"error": str(e)})

if __name__ == "__main__":
    import uvicorn
    # Log thông báo khởi động
    logger.info(f"Starting Proxy Server. Target: {TARGET_MODEL}")
    uvicorn.run(app, host="0.0.0.0", port=8001)