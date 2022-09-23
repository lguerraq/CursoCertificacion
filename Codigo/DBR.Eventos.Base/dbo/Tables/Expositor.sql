CREATE TABLE [dbo].[Expositor] (
    [IdExpositor]         INT          IDENTITY (1, 1) NOT NULL,
    [Nombres]             VARCHAR (50) NULL,
    [ApellidoPaterno]     VARCHAR (50) NULL,
    [ApellidoMaterno]     VARCHAR (50) NULL,
    [Correo]              VARCHAR (50) NULL,
    [Telefono]            VARCHAR (9)  NULL,
    [Estado]              BIT          NOT NULL,
    [UsuarioCreacion]     INT          NOT NULL,
    [FechaCreacion]       DATETIME     NOT NULL,
    [UsuarioModificacion] INT          NULL,
    [FechaModificacion]   DATETIME     NULL,
    CONSTRAINT [PK_Expositor] PRIMARY KEY CLUSTERED ([IdExpositor] ASC)
);

