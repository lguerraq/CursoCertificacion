CREATE PROCEDURE [dbo].[UspListLeccionByModuloPaged]
(
	@IdModulo INT,
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
			COUNT(p.IdModulo) OVER() TotalRegistros,
			ROW_NUMBER() OVER(ORDER BY p.IdModulo ASC) AS RowNum, 
			p.IdLeccion,
			p.IdModulo,
			p.Tipo,
			t.NombreTipo,
			p.Nombre,
            p.Descripcion,
            p.Duracion
		FROM Leccion p WITH(NOLOCK)
		INNER JOIN Tipo t WITH(NOLOCK) 
		ON t.Valor = p.Tipo AND t.Grupo = 'TIPO LECCION'
		WHERE p.Estado=1 and p.IdModulo=@IdModulo
		AND 
		(
			p.Nombre LIKE ('%' + @search + '%')
		)
	) RECORD WHERE RECORD.RowNum>@start AND RECORD.RowNum<=@end

END