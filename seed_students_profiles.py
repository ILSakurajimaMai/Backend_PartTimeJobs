import argparse
import json
import random
import sys
from datetime import date, timedelta
from pathlib import Path
from typing import Any
from urllib import error, request
from uuid import uuid4


UNIVERSITIES = [
    "Ho Chi Minh City University of Technology",
    "University of Information Technology - VNUHCM",
    "Foreign Trade University",
    "National Economics University",
    "Ton Duc Thang University",
    "FPT University",
]

MAJORS = [
    "Software Engineering",
    "Computer Science",
    "Information Systems",
    "Digital Marketing",
    "Business Administration",
    "Finance and Banking",
]

POSITIONS = [
    "Backend Intern",
    "Frontend Intern",
    "Fullstack Intern",
    "Data Analyst Intern",
    "Sales Intern",
    "Marketing Intern",
]

CAREER_TRACKS = {
    "Backend Intern": ["C#", ".NET", "SQL Server", "Entity Framework", "REST API"],
    "Frontend Intern": ["JavaScript", "React", "HTML", "CSS", "TypeScript"],
    "Fullstack Intern": ["C#", ".NET", "React", "SQL Server", "Docker"],
    "Data Analyst Intern": ["Python", "SQL", "Power BI", "Excel", "Statistics"],
    "Sales Intern": ["Sales", "CRM", "Negotiation", "Communication", "Lead Generation"],
    "Marketing Intern": ["Content Marketing", "SEO", "Google Ads", "Analytics", "Communication"],
}

COMPANIES = [
    "FPT Software",
    "VNG Corporation",
    "Shopee Vietnam",
    "Momo",
    "Viettel Solutions",
    "Tiki",
]

CERTIFICATIONS = [
    "Google Data Analytics",
    "TOEIC 850",
    "AWS Cloud Practitioner",
    "Meta Social Media Marketing",
    "MOS Excel Expert",
    "Scrum Fundamentals",
]

FIRST_NAMES = [
    "An",
    "Binh",
    "Cuong",
    "Dung",
    "Giang",
    "Hanh",
    "Khanh",
    "Linh",
    "Minh",
    "Nam",
    "Ngan",
    "Phuong",
    "Quang",
    "Trang",
    "Vy",
]

LAST_NAMES = [
    "Nguyen",
    "Tran",
    "Le",
    "Pham",
    "Hoang",
    "Phan",
    "Vu",
    "Dang",
    "Bui",
    "Do",
]

STREETS = [
    "12 Nguyen Van Linh, District 7, HCMC",
    "88 Vo Van Tan, District 3, HCMC",
    "45 Xo Viet Nghe Tinh, Binh Thanh, HCMC",
    "210 Cau Giay, Hanoi",
    "18 Le Duan, Da Nang",
    "50 Nguyen Hue, District 1, HCMC",
]


def api_key(data: dict[str, Any], *names: str) -> Any:
    for name in names:
        if name in data:
            return data[name]
    return None


class ApiError(Exception):
    def __init__(self, method: str, url: str, status_code: int, raw: str):
        self.method = method
        self.url = url
        self.status_code = status_code
        self.raw = raw
        self.payload: dict[str, Any] | None = None
        try:
            parsed = json.loads(raw)
            if isinstance(parsed, dict):
                self.payload = parsed
        except json.JSONDecodeError:
            pass
        super().__init__(f"{method} {url} failed: {status_code} {raw}")

    @property
    def message(self) -> str:
        if self.payload:
            return str(api_key(self.payload, "message", "Message") or "")
        return ""


def call_api(method: str, url: str, payload: dict[str, Any] | None = None, token: str | None = None) -> dict[str, Any]:
    body = None
    headers = {"Content-Type": "application/json", "User-Agent": "ptj-seed-script"}
    if payload is not None:
        body = json.dumps(payload).encode("utf-8")
    if token:
        headers["Authorization"] = f"Bearer {token}"

    req = request.Request(url=url, data=body, headers=headers, method=method)
    try:
        with request.urlopen(req) as resp:
            raw = resp.read().decode("utf-8")
    except error.HTTPError as exc:
        raw = exc.read().decode("utf-8", errors="ignore")
        raise ApiError(method, url, exc.code, raw) from exc
    except error.URLError as exc:
        raise RuntimeError(f"{method} {url} failed: {exc.reason}") from exc

    if not raw:
        return {}
    return json.loads(raw)


def random_full_name(rng: random.Random) -> str:
    return f"{rng.choice(LAST_NAMES)} {rng.choice(FIRST_NAMES)}"


def random_phone(index: int) -> str:
    return f"09{index:08d}"[-10:]


def random_birth_date(rng: random.Random) -> str:
    start = date(2000, 1, 1)
    end = date(2005, 12, 31)
    return (start + timedelta(days=rng.randint(0, (end - start).days))).isoformat()


def education_block(university: str, major: str, year_of_study: int, gpa: float) -> list[dict[str, Any]]:
    start_year = 2021 - max(0, year_of_study - 1)
    return [
        {
            "id": 0,
            "institutionName": university,
            "degree": "Bachelor",
            "fieldOfStudy": major,
            "startDate": f"{start_year}-09-01",
            "endDate": None,
            "gpa": round(gpa, 2),
            "description": f"Student in {major} with part-time job orientation.",
        }
    ]


def experience_block(track: str, rng: random.Random) -> list[dict[str, Any]]:
    company = rng.choice(COMPANIES)
    position = track.replace("Intern", "Collaborator")
    return [
        {
            "id": 0,
            "companyName": company,
            "position": position,
            "description": f"Supported team tasks related to {track.lower()}, reporting, and delivery coordination.",
            "startDate": "2024-06-01",
            "endDate": "2024-12-31",
            "isCurrentlyWorking": False,
        }
    ]


def skill_block(track: str, rng: random.Random) -> list[dict[str, Any]]:
    skills = CAREER_TRACKS[track][:]
    rng.shuffle(skills)
    return [
        {
            "id": 0,
            "skillName": skill,
            "proficiencyLevel": rng.randint(2, 4),
            "yearsOfExperience": rng.randint(1, 3),
        }
        for skill in skills[:5]
    ]


def certificate_block(rng: random.Random) -> list[dict[str, Any]]:
    cert = rng.choice(CERTIFICATIONS)
    return [
        {
            "id": 0,
            "name": cert,
            "issuingOrganization": cert.split()[0],
            "issueDate": "2024-08-15",
            "expiryDate": None,
            "credentialId": f"CERT-{rng.randint(100000, 999999)}",
            "credentialUrl": f"https://credentials.example.com/{rng.randint(10000, 99999)}",
            "certificateFileUrl": f"https://cdn.example.com/certificates/{rng.randint(10000, 99999)}.pdf",
        }
    ]


def build_update_profile_payload(
    full_name: str,
    email: str,
    phone_number: str,
    birth_iso: str,
    rng: random.Random,
) -> dict[str, Any]:
    return {
        "fullName": full_name,
        "email": email,
        "phoneNumber": phone_number,
        "dateOfBirth": birth_iso,
        "address": rng.choice(STREETS),
    }


def build_cv_payload(
    full_name: str,
    email: str,
    phone_number: str,
    student_id: str,
    birth_iso: str,
    cv_index: int,
    cvs_per_user: int,
    rng: random.Random,
) -> dict[str, Any]:
    track = POSITIONS[cv_index % len(POSITIONS)]
    major = rng.choice(MAJORS)
    university = rng.choice(UNIVERSITIES)
    gpa = round(rng.uniform(2.8, 3.9), 2)
    year_of_study = rng.randint(2, 4)

    return {
        "id": 0,
        "title": f"CV {track}" + (f" ({cv_index + 1})" if cvs_per_user > 1 else ""),
        "targetPosition": track,
        "isDefault": cv_index == 0,
        "fullName": full_name,
        "email": email,
        "dateOfBirth": birth_iso,
        "gender": rng.randint(0, 2),
        "address": rng.choice(STREETS),
        "phoneNumber": phone_number,
        "studentId": student_id,
        "university": university,
        "major": major,
        "gpa": gpa,
        "yearOfStudy": year_of_study,
        "expectedGraduationDate": f"{2027 + max(0, year_of_study - 2)}-06-30",
        "resumeUrl": f"https://cdn.example.com/resumes/{student_id.lower()}_{cv_index + 1}.pdf",
        "bio": f"Student aiming for {track.lower()} roles, available for part-time work and internship opportunities.",
        "linkedInUrl": f"https://linkedin.com/in/{email.split('@')[0]}",
        "gitHubUrl": f"https://github.com/{email.split('@')[0]}",
        "skills": skill_block(track, rng),
        "experiences": experience_block(track, rng),
        "educations": education_block(university, major, year_of_study, gpa),
        "certificates": certificate_block(rng),
    }


def build_register_payload(email: str, password: str, full_name: str, phone_number: str) -> dict[str, Any]:
    return {
        "email": email,
        "password": password,
        "confirmPassword": password,
        "role": "STUDENT",
        "fullName": full_name,
        "phoneNumber": phone_number,
    }


def build_login_payload(email: str, password: str) -> dict[str, Any]:
    return {"email": email, "password": password}


def generate_email(index: int, run_tag: str, attempt: int, rng: random.Random) -> str:
    suffix = rng.randint(1000, 9999)
    return f"student_seed_{run_tag}_{index}_{attempt}_{suffix}@example.com"


def result_cv_list(result: dict[str, Any]) -> list[dict[str, Any]]:
    data = api_key(result, "data", "Data")
    if isinstance(data, list):
        return [x for x in data if isinstance(x, dict)]
    return []


def create_student(
    base_url: str,
    index: int,
    cvs_per_user: int,
    password: str,
    rng: random.Random,
    run_tag: str,
) -> dict[str, Any]:
    full_name = random_full_name(rng)
    phone_number = random_phone(index)
    student_id = f"SE{run_tag.upper()}{index:04d}"[:20]
    birth_iso = random_birth_date(rng)

    register_result: dict[str, Any] | None = None
    email = ""
    for attempt in range(1, 11):
        email = generate_email(index, run_tag, attempt, rng)
        try:
            register_result = call_api(
                "POST",
                f"{base_url}/api/auth/register",
                build_register_payload(email, password, full_name, phone_number),
            )
            break
        except ApiError as exc:
            if exc.status_code == 400 and "Email already exists" in exc.message:
                continue
            raise

    if register_result is None:
        raise RuntimeError(f"Unable to create a unique account after multiple attempts for user #{index}")

    if not api_key(register_result, "success", "Success"):
        raise RuntimeError(f"Register failed for {email}: {register_result}")

    login_result = call_api(
        "POST",
        f"{base_url}/api/auth/login",
        build_login_payload(email, password),
    )
    if not api_key(login_result, "success", "Success"):
        raise RuntimeError(f"Login failed for {email}: {login_result}")

    login_data = api_key(login_result, "data", "Data") or {}
    access_token = api_key(login_data, "accessToken", "AccessToken")
    if not access_token:
        raise RuntimeError(f"Access token missing for {email}: {login_result}")

    profile_update = build_update_profile_payload(full_name, email, phone_number, birth_iso, rng)
    profile_result = call_api(
        "PUT",
        f"{base_url}/api/profile/me",
        profile_update,
        token=access_token,
    )
    if not api_key(profile_result, "success", "Success"):
        raise RuntimeError(f"Update profile failed for {email}: {profile_result}")

    list_result = call_api("GET", f"{base_url}/api/cvs/my", None, token=access_token)
    if not api_key(list_result, "success", "Success"):
        raise RuntimeError(f"List CVs failed for {email}: {list_result}")

    existing = result_cv_list(list_result)
    created_cvs: list[dict[str, Any]] = []

    for cv_index in range(cvs_per_user):
        payload = build_cv_payload(
            full_name, email, phone_number, student_id, birth_iso, cv_index, cvs_per_user, rng
        )

        if cv_index < len(existing):
            cv_id = api_key(existing[cv_index], "id", "Id")
            if cv_id is None:
                raise RuntimeError(f"CV list entry missing id for {email}: {existing[cv_index]}")
            cv_result = call_api(
                "PUT",
                f"{base_url}/api/cvs/{cv_id}",
                payload,
                token=access_token,
            )
        else:
            cv_result = call_api(
                "POST",
                f"{base_url}/api/cvs",
                payload,
                token=access_token,
            )

        if not api_key(cv_result, "success", "Success"):
            raise RuntimeError(f"CV upsert failed for {email} index {cv_index}: {cv_result}")

        cv_data = api_key(cv_result, "data", "Data") or {}
        created_cvs.append(
            {
                "id": api_key(cv_data, "id", "Id"),
                "title": api_key(cv_data, "title", "Title"),
                "targetPosition": api_key(cv_data, "targetPosition", "TargetPosition"),
                "isDefault": api_key(cv_data, "isDefault", "IsDefault"),
            }
        )

    return {
        "email": email,
        "password": password,
        "fullName": full_name,
        "phoneNumber": phone_number,
        "studentId": student_id,
        "cvs": created_cvs,
    }


def parse_args() -> argparse.Namespace:
    parser = argparse.ArgumentParser(description="Seed STUDENT accounts, profile (PUT /api/profile/me), and CVs (PTJ API).")
    parser.add_argument("--base-url", default="http://localhost:5000", help="Backend base URL")
    parser.add_argument("--users", type=int, default=10, help="Number of student accounts")
    parser.add_argument("--cvs-per-user", type=int, default=3, help="CVs per student (default CV from register is updated first)")
    parser.add_argument("--password", default="Seed123!", help="Password used for all accounts")
    parser.add_argument("--seed", type=int, default=20260420, help="Random seed")
    parser.add_argument(
        "--run-tag",
        default=uuid4().hex[:8],
        help="Unique tag added to generated emails and student IDs",
    )
    parser.add_argument(
        "--output",
        default="seed_students_profiles_output.json",
        help="Output JSON file with created accounts",
    )
    return parser.parse_args()


def main() -> int:
    args = parse_args()
    rng = random.Random(args.seed)
    base_url = args.base_url.rstrip("/")

    if args.users <= 0:
        print("--users must be > 0", file=sys.stderr)
        return 1
    if args.cvs_per_user <= 0:
        print("--cvs-per-user must be > 0", file=sys.stderr)
        return 1

    print(f"run_tag={args.run_tag}")
    results: list[dict[str, Any]] = []
    for index in range(1, args.users + 1):
        account = create_student(base_url, index, args.cvs_per_user, args.password, rng, args.run_tag)
        results.append(account)
        print(f"[{index}/{args.users}] created {account['email']} with {len(account['cvs'])} cvs")

    output_path = Path(args.output)
    output_path.write_text(json.dumps(results, indent=2), encoding="utf-8")
    print(f"saved {len(results)} accounts to {output_path}")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
