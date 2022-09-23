CREATE PROCEDURE ListHistorialActividadUsuario
AS
BEGIN	
	SELECT 
		ut.Descripcion TipoUsuario,
		u.Nombres,
		u.ApellidoPaterno + ' ' + u.ApellidoMaterno Apellidos,	
		DATEADD(HOUR,2,uh.FechaCreacion ) FechaIngreso
	FROM dbo.UsuarioHistorico uh 
	INNER JOIN dbo.Usuario u ON uh.IdUsuario=u.IdUsuario
	INNER JOIN dbo.UsuarioTipo ut ON u.IdUsuarioTipo=ut.IdUsuarioTipo
	WHERE u.IdUsuario<>1
	ORDER BY u.IdUsuario,uh.FechaCreacion

END