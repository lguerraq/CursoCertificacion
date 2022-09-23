CREATE TABLE [dbo].[Usuario] (
    [IdUsuario]           INT          IDENTITY (1, 1) NOT NULL,
    [Login]               VARCHAR (50) NULL,
    [Password]            VARCHAR (50) NULL,
    [NumeroDocumento]     VARCHAR (20) NULL,
    [Nombres]             VARCHAR (50) NULL,
    [ApellidoPaterno]     VARCHAR (50) NULL,
    [ApellidoMaterno]     VARCHAR (50) NULL,
    [IdUsuarioTipo]       INT          NULL,
    [Estado]              BIT          NOT NULL,
    [UsuarioCreacion]     INT          NOT NULL,
    [FechaCreacion]       DATETIME     NOT NULL,
    [UsuarioModificacion] INT          NULL,
    [FechaModificacion]   DATETIME     NULL,
    CONSTRAINT [PK_Usuario] PRIMARY KEY CLUSTERED ([IdUsuario] ASC),
    CONSTRAINT [FK_Usuario_UsuarioTipo] FOREIGN KEY ([IdUsuarioTipo]) REFERENCES [dbo].[UsuarioTipo] ([IdUsuarioTipo])
);



