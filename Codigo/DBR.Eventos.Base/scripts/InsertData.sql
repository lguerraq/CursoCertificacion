USE [db_ingenieros]
GO

SET IDENTITY_INSERT [dbo].[Tipo] ON 
GO
INSERT [dbo].[Tipo] ([IdTipo], [NombreTipo], [Grupo], [Valor], [Abreviatura]) VALUES (25, N'CURSO', N'TIPO CURSO', 1, NULL)
GO
INSERT [dbo].[Tipo] ([IdTipo], [NombreTipo], [Grupo], [Valor], [Abreviatura]) VALUES (26, N'TALLER', N'TIPO CURSO', 2, NULL)
GO
INSERT [dbo].[Tipo] ([IdTipo], [NombreTipo], [Grupo], [Valor], [Abreviatura]) VALUES (27, N'SEMINARIO', N'TIPO CURSO', 3, NULL)
GO
INSERT [dbo].[Tipo] ([IdTipo], [NombreTipo], [Grupo], [Valor], [Abreviatura]) VALUES (28, N'CONFERENCIA', N'TIPO CURSO', 4, NULL)
GO
INSERT [dbo].[Tipo] ([IdTipo], [NombreTipo], [Grupo], [Valor], [Abreviatura]) VALUES (29, N'TEORÍA', N'TIPO LECCION', 1, NULL)
GO
INSERT [dbo].[Tipo] ([IdTipo], [NombreTipo], [Grupo], [Valor], [Abreviatura]) VALUES (30, N'PRÁCTICA', N'TIPO LECCION', 2, NULL)
GO
INSERT [dbo].[Tipo] ([IdTipo], [NombreTipo], [Grupo], [Valor], [Abreviatura]) VALUES (31, N'EVALUACIÓN', N'TIPO LECCION', 3, NULL)
GO
SET IDENTITY_INSERT [dbo].[Tipo] OFF


INSERT INTO [dbo].[Opcion]
           ([IdPadre]
           ,[Icono]
           ,[Descipcion]
           ,[UrlDescripcion]
           ,[Orden]
           ,[Estado]
           ,[FechaCreacion]
           ,[UsuarioCreacion]
           ,[FechaModificacion]
           ,[UsuarioModificacion])
     VALUES
           (NULL
           ,'uil uil-apps'
           ,'Cursos'
           ,'Aula/Cursos/Matriculados'
           ,0
           ,1
           ,GETDATE()
           ,1
           ,NULL
           ,NULL)
GO

INSERT INTO [dbo].[OpcionUsuarioTipo]
           ([IdOpcion]
           ,[IdUsuarioTipo]
           ,[Estado]
           ,[FechaCreacion]
           ,[UsuarioCreacion]
           ,[FechaModificacion]
           ,[UsuarioModificacion])
     VALUES
           (17 /*Verificar el id generado en el script anterior*/
           ,4
           ,NULL
           ,GETDATE()
           ,1
           ,NULL
           ,NULL)
GO
