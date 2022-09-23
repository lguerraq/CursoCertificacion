CREATE TABLE [dbo].[Desafiliado] (
    [IdDesafiliado]       INT           IDENTITY (1, 1) NOT NULL,
    [Correo]              VARCHAR (250) NULL,
    [Valor]               INT           NULL,
    [Observacion]         VARCHAR (250) NULL,
    [Estado]              BIT           NOT NULL,
    [UsuarioCreacion]     INT           NOT NULL,
    [FechaCreacion]       DATETIME      NOT NULL,
    [UsuarioModificacion] INT           NULL,
    [FechaModificacion]   DATETIME      NULL,
    CONSTRAINT [PK_Desafiliado] PRIMARY KEY CLUSTERED ([IdDesafiliado] ASC)
);

