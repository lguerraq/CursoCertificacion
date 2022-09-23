CREATE PROCEDURE [dbo].[ListEventoUsuarioVirtualVideoByEvento]
(
	@IdEvento INT,
	@IdEventoUsuario INT
)
AS
BEGIN
	SELECT 
		vv.IdVirtualVideo,
        vv.IdEvento,
        vv.Url,
        euvv.Id IdEventoUsuarioVirtualVideo
	FROM VirtualVideo vv 
	LEFT JOIN EventoUsuarioVirtualVideo euvv on vv.IdVirtualVideo=euvv.IdVirtualVideo AND euvv.Estado=1 AND euvv.IdEventoUsuario=@IdEventoUsuario
	WHERE vv.IdEvento=@IdEvento and vv.Estado=1 
END