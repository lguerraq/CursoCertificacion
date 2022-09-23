CREATE TABLE [dbo].[VirtualVideo] (
    [IdVirtualVideo]      INT           IDENTITY (1, 1) NOT NULL,
    [IdEvento]            INT           NULL,
    [Url]                 VARCHAR (250) NULL,
    [Estado]              BIT           NOT NULL,
    [UsuarioCreacion]     INT           NOT NULL,
    [FechaCreacion]       DATETIME      NOT NULL,
    [UsuarioModificacion] INT           NULL,
    [FechaModificacion]   DATETIME      NULL,
    CONSTRAINT [PK_VirtualVideo] PRIMARY KEY CLUSTERED ([IdVirtualVideo] ASC),
    CONSTRAINT [FK_VirtualVideo_Evento] FOREIGN KEY ([IdEvento]) REFERENCES [dbo].[Evento] ([IdEvento])
);

