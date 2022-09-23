CREATE PROCEDURE [dbo].[UspListPersonaPaged]
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
			COUNT(p.IdPersona) OVER() TotalRegistros,
			ROW_NUMBER() OVER(ORDER BY p.IdPersona DESC) AS RowNum, 
			p.IdPersona,
            p.Nombres,
            p.ApellidoPaterno,
            p.ApellidoMaterno,
            p.NumeroDocumento,
            p.TipoOcupacion,
            p.DescripcionOcupacion,
            ocu.NombreTipo TipoOcupacionNombre,
            ocu.Abreviatura TipoOcupacionAbreviatura,
            p.CIP,
            p.Celular,
             p.Correo,
            p.IdProfesion,
            pf.Descripcion,
			p.IdPais,
			p.Ciudad
		FROM persona p WITH(NOLOCK)
		INNER JOIN Tipo ocu WITH(NOLOCK) ON p.TipoOcupacion=ocu.Valor
		LEFT JOIN Profesion pf WITH(NOLOCK) ON p.IdProfesion=pf.IdProfesion
		WHERE p.Estado=1 AND ocu.Grupo = 'OCUPACION'
		AND 
		(
			p.Nombres LIKE ('%' + @search + '%') OR
            p.ApellidoMaterno LIKE  ('%' + @search + '%') OR
            p.ApellidoMaterno LIKE ('%' + @search + '%') OR
            p.NumeroDocumento LIKE ('%' + @search + '%')
		)
	) RECORD WHERE RECORD.RowNum>@start AND RECORD.RowNum<=@end

END