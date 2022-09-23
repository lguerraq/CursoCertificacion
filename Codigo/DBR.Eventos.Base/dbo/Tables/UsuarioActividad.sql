CREATE TABLE [dbo].[UsuarioActividad] (
    [IdUsuarioActividad] INT           IDENTITY (1, 1) NOT NULL,
    [IdUsuario]          INT           NULL,
    [FechaCreacion]      DATETIME      NULL,
    [Token]              VARCHAR (250) NULL,
    CONSTRAINT [PK_UsuarioActividad] PRIMARY KEY CLUSTERED ([IdUsuarioActividad] ASC),
    CONSTRAINT [FK_UsuarioActividad_Usuario] FOREIGN KEY ([IdUsuario]) REFERENCES [dbo].[Usuario] ([IdUsuario])
);

