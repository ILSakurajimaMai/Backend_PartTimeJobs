SELECT 
    dp.name AS RoleName, 
    p.permission_name AS PermissionType, 
    p.state_desc AS PermissionState,
    COALESCE(o.name, s.name) AS ObjectOrSchemaName,
    p.class_desc AS ObjectClass
FROM sys.database_permissions p
JOIN sys.database_principals dp ON p.grantee_principal_id = dp.principal_id
LEFT JOIN sys.objects o ON p.major_id = o.object_id
LEFT JOIN sys.schemas s ON p.major_id = s.schema_id AND p.class = 3
WHERE dp.name IN ('AppAdminRole', 'AppEmployerRole', 'AppStudentRole')
ORDER BY RoleName, ObjectClass, ObjectOrSchemaName;