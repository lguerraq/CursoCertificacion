CREATE PROCEDURE [dbo].[UspListCorreoPaged]
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
		RECORD.*,
		(SELECT COUNT(1) FROM CorreoDifusion WITH(NOLOCK) WHERE IdCorreo=RECORD.IdCorreo) Cantidad
	FROM
	(
		SELECT 
			COUNT(1) OVER() TotalRegistros,
			ROW_NUMBER() OVER(ORDER BY p.IdCorreo DESC) AS RowNum, 
			p.IdCorreo,
            p.Asunto,
            p.Origen,
            p.NombreOrigen,
            p.EstadoCorreo,
            p.FechaEnvio,
            p.NumeroEnvio           
		FROM Correo p WITH(NOLOCK)
		WHERE p.Estado=1
	) RECORD WHERE RECORD.RowNum>@start AND RECORD.RowNum<=@end
	ORDER BY RECORD.IdCorreo DESC
END