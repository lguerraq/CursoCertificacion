CREATE PROCEDURE [dbo].[UspListUsuarioPaged]
(
	@search VARCHAR(20),
	@start INT,
	@length INT
)
AS
BEGIN

	DECLARE @end INT;

	SET @end=(@start+@length);

	SELECT 
		RECORD.* 
	FROM
	(
		SELECT 
			COUNT(p.IdUsuario) OVER() TotalRegistros,
			ROW_NUMBER() OVER(ORDER BY p.IdUsuario DESC) AS RowNum, 
			p.IdUsuario,
			p.Login,
			p.Password,
            p.Nombres,
            p.ApellidoPaterno,
            p.ApellidoMaterno,
			p.IdUsuarioTipo,
			ocu.Descripcion UsuarioTipo
		FROM Usuario p WITH(NOLOCK)
		INNER JOIN UsuarioTipo ocu WITH(NOLOCK) ON p.IdUsuarioTipo=ocu.IdUsuarioTipo
		WHERE p.Estado=1 and p.IdUsuario>1
		AND 
		(
			p.Nombres LIKE ('%' + @search + '%') OR
            p.ApellidoMaterno LIKE  ('%' + @search + '%') OR
            p.ApellidoMaterno LIKE ('%' + @search + '%') OR
            p.Login LIKE ('%' + @search + '%')
		)
	) RECORD WHERE RECORD.RowNum>@start AND RECORD.RowNum<=@end

END