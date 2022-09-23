CREATE PROCEDURE [dbo].[UspListModuloByEventoPaged]
(
	@IdEvento INT,
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
			p.IdModulo,
			p.IdEvento,
			p.Nombre,
            p.Descripcion,
			p.Expositor,
            p.Horas
		FROM Modulo p WITH(NOLOCK)
		WHERE p.Estado=1 and p.IdEvento=@IdEvento
		AND 
		(
			p.Nombre LIKE ('%' + @search + '%')
		)
	) RECORD WHERE RECORD.RowNum>@start AND RECORD.RowNum<=@end

END