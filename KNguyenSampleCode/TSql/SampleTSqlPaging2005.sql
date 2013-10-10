set ansi_nulls on
go
set quoted_identifier on
go
set ansi_padding on
go


/* sql 2005 & above -- create table in memory to store sample data */
declare @Product table (	
	[ProductId] [int] identity(1,1) NOT NULL,
	[Name] [nvarchar](400) NOT NULL,
	[Description] [ntext] NULL,
	[Sku] [nvarchar](50) NULL,
	[Price] [money] NOT NULL
)

/* populate table with sample data */
insert into @Product([Name],[Description],[Sku],[Price]) values ( 'Scissors', 'Left-handed', 'PS32556', 3.00)
insert into @Product([Name],[Description],[Sku],[Price]) values ( 'Pens', 'Standard 10-pack', 'PN35225', 4.50)
insert into @Product([Name],[Description],[Sku],[Price]) values ( 'Pencils', 'Standard 10-pack', 'SB35235', 1.00)
insert into @Product([Name],[Description],[Sku],[Price]) values ( 'Markers', 'Standard 5-pack', 'MR3295', 5.00)
insert into @Product([Name],[Description],[Sku],[Price]) values ( 'Highlighters', 'Standard 3-pack', 'HL3255', 5.00)
insert into @Product([Name],[Description],[Sku],[Price]) values ( 'Tape', 'Duct tape 3-pack', 'DT3255', 7.00)
insert into @Product([Name],[Description],[Sku],[Price]) values ( 'Rubber Bands', '200-pack', 'RB3255', 7.50)
insert into @Product([Name],[Description],[Sku],[Price]) values ( 'Standard Paper', 'College-ruled Paper', 'SA33735', 1.00)
insert into @Product([Name],[Description],[Sku],[Price]) values ( '200-page Notebook', 'College-rule Notebook', 'NB3275', 1.50)
insert into @Product([Name],[Description],[Sku],[Price]) values ( '100-page Notebook', 'Wide-rule Notebook', 'WR3275', 1.75)
insert into @Product([Name],[Description],[Sku],[Price]) values ( 'Stapler', 'Wide-ruled Notebook', 'ST3232', 1.75)
insert into @Product([Name],[Description],[Sku],[Price]) values ( 'Spiral notebooks', 'Standard', 'SN2962', 1.75)
insert into @Product([Name],[Description],[Sku],[Price]) values ( 'Writing pads', 'Standard', 'WP2962', 1.75)
insert into @Product([Name],[Description],[Sku],[Price]) values ( 'Post-it notes', 'Standard', 'PT2962', 2.00)
insert into @Product([Name],[Description],[Sku],[Price]) values ( 'Phone message pads', 'Standard', 'PM2962', 1.75)
insert into @Product([Name],[Description],[Sku],[Price]) values ( 'Laser printer paper', 'Standard', 'LP4392', 1.75)
insert into @Product([Name],[Description],[Sku],[Price]) values ( 'Copy paper', 'Standard', 'CP2442', 2.00)
insert into @Product([Name],[Description],[Sku],[Price]) values ( 'Fax paper', 'Standard', 'FP2212', 3.00)
insert into @Product([Name],[Description],[Sku],[Price]) values ( 'Graph paper', 'Standard', 'GP2572', 1.50)
insert into @Product([Name],[Description],[Sku],[Price]) values ( 'Colored paper', 'Standard', 'CP3252', 5.00)
insert into @Product([Name],[Description],[Sku],[Price]) values ( 'Pocket notebook', 'Standard', 'PN3251', 6.00)

declare @intPageIndex int; set @intPageIndex=1 --first page (base-1)
declare @intPageSize int; set @intPageSize=5 --page size of results to return

select * from (
	select [ProductId], [Name], [Description], [Sku], [Price], [RowNum] = row_number() over (order by [Name]) 
	from @Product
	where [Description] like '%Standard%' --filter by description
	and Price > 1.00 --filter by price over $1
) t
where [RowNum] > (@intPageIndex-1)*@intPageSize and [RowNum] <= (@intPageIndex-1)*@intPageSize+@intPageSize

go

