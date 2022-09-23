CREATE TABLE [dbo].[Evento] (
    [IdEvento]                      INT              IDENTITY (1, 1) NOT NULL,
    [Tipo]                          INT              NULL,
    [Modalidad]                     INT              NULL,
    [NombreEvento]                  VARCHAR (250)    NULL,
    [Descripcion]                   VARCHAR (1000)   NULL,
    [Fecha]                         DATETIME         NULL,
    [Expositor]                     VARCHAR (MAX)    NULL,
    [ImagenEvento]                  VARCHAR (150)    NULL,
    [DocumentoFotocheck]            VARCHAR (150)    NULL,
    [DocumentoCertificado]          VARCHAR (150)    NULL,
    [DocumentoCertificadoImprimir]  VARCHAR (150)    NULL,
    [DocumentoCertificadoExpositor] VARCHAR (150)    NULL,
    [Horas]                         INT              NULL,
    [Activo]                        BIT              NULL,
    [NotaAprobatoria]               DECIMAL (12, 2)  NULL,
    [Costo]                         VARCHAR (10)     NULL,
    [CostoValor]                    DECIMAL (12, 2)  NULL,
    [CostoValorPromocional]         DECIMAL (12, 2)  NULL,
    [Estado]                        BIT              NOT NULL,
    [rowid]                         UNIQUEIDENTIFIER CONSTRAINT [DF_Evento_rowid] DEFAULT (newid()) NULL,
    [UsuarioCreacion]               INT              NOT NULL,
    [FechaCreacion]                 DATETIME         NOT NULL,
    [UsuarioModificacion]           INT              NULL,
    [FechaModificacion]             DATETIME         NULL,
    CONSTRAINT [PK_Evento] PRIMARY KEY CLUSTERED ([IdEvento] ASC)
);













