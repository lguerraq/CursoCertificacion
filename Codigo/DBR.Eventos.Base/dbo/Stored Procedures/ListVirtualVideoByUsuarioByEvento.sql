CREATE PROCEDURE [dbo].[ListVirtualVideoByUsuarioByEvento]
(
	@IdEvento INT,
	@IdUsuario INT
)
AS
BEGIN
	DECLARE @IdTipoUsuario INT;

	SET @IdTipoUsuario=(SELECT u.IdUsuarioTipo FROM Usuario u WHERE u.IdUsuario=@IdUsuario);

	IF(@IdTipoUsuario=1 OR @IdTipoUsuario=2)
		BEGIN
			SELECT 
				vv.IdVirtualVideo,
				vv.IdEvento,
				vv.Url,
				CAST(1 AS BIT) MostrarVideo
			FROM VirtualVideo vv 
			WHERE vv.IdEvento=@IdEvento and vv.Estado=1
		END
	ELSE
		BEGIN
			SELECT 
				vv.IdVirtualVideo,
				vv.IdEvento,
				vv.Url,
				(CASE WHEN RECORD.Id IS NOT NULL THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END) MostrarVideo
			FROM VirtualVideo vv 
			LEFT JOIN (SELECT 
							euvv.Id,
							euvv.IdVirtualVideo
						FROM EventoUsuario eu 
						INNER JOIN EventoUsuarioVirtualVideo euvv ON eu.IdEventoUsuario=euvv.IdEventoUsuario
						WHERE eu.Estado=1 AND euvv.Estado=1 AND eu.IdEvento=@IdEvento AND eu.IdUsuario=@IdUsuario) RECORD ON RECORD.IdVirtualVideo=vv.IdVirtualVideo
			WHERE vv.IdEvento=@IdEvento and vv.Estado=1
		END
	
END