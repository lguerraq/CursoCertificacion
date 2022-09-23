CREATE PROCEDURE [dbo].[UspListPortadaPaged]
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
			COUNT(p.IdPortada) OVER() TotalRegistros,
			ROW_NUMBER() OVER(ORDER BY p.IdPortada ASC) AS RowNum, 
			p.IdPortada,
            p.NombreImagen,
            p.Descripcion
		FROM Portada p WITH(NOLOCK)
		WHERE p.Estado=1
	) RECORD WHERE RECORD.RowNum>@start AND RECORD.RowNum<=@end

END