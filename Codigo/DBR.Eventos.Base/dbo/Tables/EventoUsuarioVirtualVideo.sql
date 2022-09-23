CREATE TABLE [dbo].[EventoUsuarioVirtualVideo] (
    [Id]                  INT      IDENTITY (1, 1) NOT NULL,
    [IdEventoUsuario]     INT      NULL,
    [IdVirtualVideo]      INT      NULL,
    [Estado]              BIT      NOT NULL,
    [UsuarioCreacion]     INT      NOT NULL,
    [FechaCreacion]       DATETIME NOT NULL,
    [UsuarioModificacion] INT      NULL,
    [FechaModificacion]   DATETIME NULL,
    CONSTRAINT [PK_EventoUsuarioVirtualVideo] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_EventoUsuarioVirtualVideo_EventoUsuario] FOREIGN KEY ([IdEventoUsuario]) REFERENCES [dbo].[EventoUsuario] ([IdEventoUsuario]),
    CONSTRAINT [FK_EventoUsuarioVirtualVideo_VirtualVideo] FOREIGN KEY ([IdVirtualVideo]) REFERENCES [dbo].[VirtualVideo] ([IdVirtualVideo])
);

