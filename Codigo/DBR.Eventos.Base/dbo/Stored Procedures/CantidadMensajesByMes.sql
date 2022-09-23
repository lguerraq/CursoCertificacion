
CREATE PROCEDURE [dbo].[CantidadMensajesByMes]
AS
BEGIN
	SELECT
		YEAR(FechaCreacion) Año,
		CASE
			WHEN MONTH(FechaCreacion)=1 THEN 'ENERO'
			WHEN MONTH(FechaCreacion)=2 THEN 'FEBRERO'
			WHEN MONTH(FechaCreacion)=3 THEN 'MARZO'
			WHEN MONTH(FechaCreacion)=4 THEN 'ABRIL'
			WHEN MONTH(FechaCreacion)=5 THEN 'MAYO'
			WHEN MONTH(FechaCreacion)=6 THEN 'JUNIO'
			WHEN MONTH(FechaCreacion)=7 THEN 'JULIO'
			WHEN MONTH(FechaCreacion)=8 THEN 'AGOSTO'
			WHEN MONTH(FechaCreacion)=9 THEN 'SEPTIEMBRE'
			WHEN MONTH(FechaCreacion)=10 THEN 'OCTUBRE'
			WHEN MONTH(FechaCreacion)=11 THEN 'NOVIEMBRE'
			WHEN MONTH(FechaCreacion)=12 THEN 'DICIEMBRE'
			END Mes, 
		COUNT(1) TotalEnviados,
		CASE WHEN COUNT(1)>10000 THEN 10000 ELSE COUNT(1) END  EnviadosGratis,
		CASE 
			WHEN (COUNT(1)-10000)>0 THEN (COUNT(1)-10000)
			ELSE 0 END EnviadosAdicionales,
		0.005 CostoEnvioAdicional,
		CASE 
			WHEN (COUNT(1)-10000)>0 THEN (COUNT(1)-10000)/200.00
			ELSE 0 END TotalCostoAdicional
	FROM CorreoDifusion
	WHERE Estado=1
	GROUP by YEAR(FechaCreacion),MONTH(FechaCreacion)
	ORDER by MONTH(FechaCreacion)
END