CREATE PROCEDURE [dbo].[ListCantidadMensajesByMes]
AS
BEGIN
	SELECT
		CAST(ROW_NUMBER() OVER(ORDER BY MONTH(FechaCreacion) DESC) AS INT) AS Row,
		CAST(YEAR(FechaCreacion) AS INT) Año,
		CAST(MONTH(FechaCreacion) AS INT) Mes,
		COUNT(1) TotalEnviados,
		CASE WHEN COUNT(1)>15000 THEN 15000 ELSE COUNT(1) END  EnviadosGratis,
		CASE 
			WHEN (COUNT(1)-15000)>0 THEN (COUNT(1)-15000)
			ELSE 0 END EnviadosAdicionales,
		0.005 CostoEnvioAdicional,
		CASE 
			WHEN (COUNT(1)-15000)>0 THEN (COUNT(1)-15000)/200.00
			ELSE 0 END TotalCostoAdicional
	FROM CorreoDifusion WITH(NOLOCK)
	WHERE Estado=1 AND Pago IS NULL
	GROUP by YEAR(FechaCreacion),MONTH(FechaCreacion)
END