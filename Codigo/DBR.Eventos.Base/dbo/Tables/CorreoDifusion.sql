CREATE TABLE [dbo].[CorreoDifusion] (
    [IdCorreoDifusion] INT           IDENTITY (1, 1) NOT NULL,
    [IdCorreo]         INT           NULL,
    [IdEvento]         INT           NULL,
    [IdPersona]        INT           NOT NULL,
    [Correo]           VARCHAR (200) NOT NULL,
    [Estado]           INT           NOT NULL,
    [ErrorMensaje]     VARCHAR (MAX) NULL,
    [ErrorStackTrace]  VARCHAR (MAX) NULL,
    [UsuarioCreacion]  INT           NOT NULL,
    [FechaCreacion]    DATETIME      NOT NULL,
    [NumeroEnvio]      INT           NULL,
    [Pago]             BIT           NULL,
    CONSTRAINT [PK__CorreoDi__80B7FCF21D16A575] PRIMARY KEY CLUSTERED ([IdCorreoDifusion] ASC),
    CONSTRAINT [FK_CorreoDifusion_Correo] FOREIGN KEY ([IdCorreo]) REFERENCES [dbo].[Correo] ([IdCorreo])
);





