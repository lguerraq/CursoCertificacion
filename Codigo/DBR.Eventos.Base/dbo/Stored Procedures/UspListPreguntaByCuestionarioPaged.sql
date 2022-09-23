CREATE PROCEDURE [dbo].[UspListPreguntaByCuestionarioPaged]
(
	@IdCuestionario INT,
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
			COUNT(p.IdPregunta) OVER() TotalRegistros,
			ROW_NUMBER() OVER(ORDER BY p.IdPregunta ASC) AS RowNum, 
			p.IdPregunta,
			p.Nombre,
            p.Explicacion,
			p.Ayuda,
            p.Puntaje
		FROM Pregunta p WITH(NOLOCK)
		WHERE p.Estado=1 and p.IdCuestionario=@IdCuestionario
		AND 
		(
			p.Nombre LIKE ('%' + @search + '%')
		)
	) RECORD WHERE RECORD.RowNum>@start AND RECORD.RowNum<=@end

END