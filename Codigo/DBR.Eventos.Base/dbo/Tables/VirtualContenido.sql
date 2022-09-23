CREATE TABLE [dbo].[VirtualContenido] (
    [IdVirtualContenido]  INT            IDENTITY (1, 1) NOT NULL,
    [IdEvento]            INT            NULL,
    [Contenido]           NVARCHAR (MAX) NULL,
    [Estado]              BIT            NOT NULL,
    [UsuarioCreacion]     INT            NOT NULL,
    [FechaCreacion]       DATETIME       NOT NULL,
    [UsuarioModificacion] INT            NULL,
    [FechaModificacion]   DATETIME       NULL,
    CONSTRAINT [PK_VirtualContenido] PRIMARY KEY CLUSTERED ([IdVirtualContenido] ASC),
    CONSTRAINT [FK_VirtualContenido_Evento] FOREIGN KEY ([IdEvento]) REFERENCES [dbo].[Evento] ([IdEvento])
);



