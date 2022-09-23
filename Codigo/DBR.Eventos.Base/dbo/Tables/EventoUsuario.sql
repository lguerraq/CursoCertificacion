CREATE TABLE [dbo].[EventoUsuario] (
    [IdEventoUsuario]     INT             IDENTITY (1, 1) NOT NULL,
    [IdEvento]            INT             NULL,
    [IdUsuario]           INT             NULL,
    [FechaInicio]         DATETIME        NULL,
    [FechaFin]            DATETIME        NULL,
    [FechaUltimoAcceso]   DATETIME        NULL,
    [NotaFinal]           DECIMAL (12, 2) NULL,
    [Abierto]             BIT             NULL,
    [Estado]              BIT             NOT NULL,
    [UsuarioCreacion]     INT             NOT NULL,
    [FechaCreacion]       DATETIME        NOT NULL,
    [UsuarioModificacion] INT             NULL,
    [FechaModificacion]   DATETIME        NULL,
    CONSTRAINT [PK_EventoUsuario] PRIMARY KEY CLUSTERED ([IdEventoUsuario] ASC),
    CONSTRAINT [FK_EventoUsuario_Evento] FOREIGN KEY ([IdEvento]) REFERENCES [dbo].[Evento] ([IdEvento]),
    CONSTRAINT [FK_EventoUsuario_Usuario] FOREIGN KEY ([IdUsuario]) REFERENCES [dbo].[Usuario] ([IdUsuario])
);





