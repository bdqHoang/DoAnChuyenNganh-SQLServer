USE [master]
GO
/****** Object:  Database [GearShop]    Script Date: 11/22/2022 8:05:48 PM ******/
CREATE DATABASE [GearShop]
GO
ALTER DATABASE [GearShop] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [GearShop] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [GearShop] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [GearShop] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [GearShop] SET ARITHABORT OFF 
GO
ALTER DATABASE [GearShop] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [GearShop] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [GearShop] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [GearShop] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [GearShop] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [GearShop] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [GearShop] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [GearShop] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [GearShop] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [GearShop] SET  ENABLE_BROKER 
GO
ALTER DATABASE [GearShop] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [GearShop] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [GearShop] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [GearShop] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [GearShop] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [GearShop] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [GearShop] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [GearShop] SET RECOVERY FULL 
GO
ALTER DATABASE [GearShop] SET  MULTI_USER 
GO
ALTER DATABASE [GearShop] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [GearShop] SET DB_CHAINING OFF 
GO
ALTER DATABASE [GearShop] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [GearShop] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [GearShop] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [GearShop] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'GearShop', N'ON'
GO
ALTER DATABASE [GearShop] SET QUERY_STORE = OFF
GO
USE [GearShop]
GO
/****** Object:  Table [dbo].[Cart]    Script Date: 11/22/2022 8:05:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cart](
	[CustomerID] [varchar](11) NOT NULL,
	[ProductID] [varchar](11) NOT NULL,
	[ColorID] [varchar](11) NOT NULL,
	[OptionID] [varchar](11) NOT NULL,
	[Quantity] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[CustomerID] ASC,
	[ProductID] ASC,
	[ColorID] ASC,
	[OptionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Categorize]    Script Date: 11/22/2022 8:05:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categorize](
	[CategorizeID] [varchar](11) NOT NULL,
	[DisplayName] [nvarchar](50) NULL,
	[CreatedAt] [datetime] NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[UpdateAt] [datetime] NULL,
	[UpdateBy] [nvarchar](50) NULL,
	[Status] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[CategorizeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Color]    Script Date: 11/22/2022 8:05:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Color](
	[ColorID] [varchar](11) NOT NULL,
	[DisplayName] [nvarchar](50) NULL,
	[CreatedAt] [datetime] NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[UpdateAt] [datetime] NULL,
	[UpdateBy] [nvarchar](50) NULL,
	[Status] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ColorID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Customer]    Script Date: 11/22/2022 8:05:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customer](
	[CustomerID] [varchar](11) NOT NULL,
	[DisplayName] [nvarchar](50) NULL,
	[Email] [varchar](30) NULL,
	[Password] [varchar](50) NULL,
	[Phone] [varchar](11) NULL,
	[CreatedAt] [datetime] NULL,
	[UpdateAt] [datetime] NULL,
	[UpdateBy] [nvarchar](50) NULL,
	[Status] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[CustomerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CustomerAddress]    Script Date: 11/22/2022 8:05:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomerAddress](
	[AddressID] [varchar](11) NOT NULL,
	[CustomerID] [varchar](11) NOT NULL,
	[AddressDetail] [char](1) NULL,
PRIMARY KEY CLUSTERED 
(
	[AddressID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Discount]    Script Date: 11/22/2022 8:05:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Discount](
	[DiscountID] [varchar](11) NOT NULL,
	[DisplayName] [nvarchar](50) NULL,
	[DiscountPersent] [float] NULL,
	[NumberOfUse] [int] NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[CreatedAt] [datetime] NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[UpdateAt] [datetime] NULL,
	[UpdateBy] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[DiscountID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Employee]    Script Date: 11/22/2022 8:05:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Employee](
	[EmployeeID] [varchar](11) NOT NULL,
	[PositionID] [varchar](11) NOT NULL,
	[UpdateAt] [datetime] NULL,
	[DisplayName] [nvarchar](50) NULL,
	[UserName] [varchar](50) NULL,
	[Password] [varchar](50) NULL,
	[Email] [varchar](50) NULL,
	[HouseNumber] [varchar](30) NULL,
	[Address] [varchar](50) NULL,
	[DateOfBirth] [datetime] NULL,
	[Phone] [varchar](11) NULL,
	[CreatedAt] [datetime] NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[UpdateBy] [nvarchar](50) NULL,
	[Status] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[EmployeeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FeedBack]    Script Date: 11/22/2022 8:05:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FeedBack](
	[ProductID] [varchar](11) NOT NULL,
	[CustomerID] [varchar](11) NOT NULL,
	[Rating] [int] NULL,
	[LikeProduct] [bit] NULL,
	[Content] [nvarchar](200) NULL,
PRIMARY KEY CLUSTERED 
(
	[ProductID] ASC,
	[CustomerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Image]    Script Date: 11/22/2022 8:05:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Image](
	[ImageID] [varchar](11) NOT NULL,
	[ProductID] [varchar](11) NOT NULL,
	[Image] [image] NULL,
	[CreatedAt] [datetime] NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[UpdateAt] [datetime] NULL,
	[UpdateBy] [nvarchar](50) NULL,
	[Ordinal] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ImageID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Invoice]    Script Date: 11/22/2022 8:05:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Invoice](
	[InvoiceID] [varchar](11) NOT NULL,
	[EmployeeID] [varchar](11) NOT NULL,
	[OrderID] [varchar](11) NOT NULL,
	[Total] [float] NULL,
	[TotalDiscount] [float] NULL,
	[TotalPayment] [float] NULL,
	[CreatedAt] [datetime] NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[UpdateAt] [datetime] NULL,
	[UpdateBy] [nvarchar](50) NULL,
	[Status] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[InvoiceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Options]    Script Date: 11/22/2022 8:05:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Options](
	[OptionID] [varchar](11) NOT NULL,
	[DisplayName] [nvarchar](50) NULL,
	[CreatedAt] [datetime] NULL,
	[CreatedBy] [varchar](50) NULL,
	[UpdateAt] [datetime] NULL,
	[UpdateBy] [nvarchar](50) NULL,
	[Status] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[OptionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Order]    Script Date: 11/22/2022 8:05:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Order](
	[OrderID] [varchar](11) NOT NULL,
	[DiscountID] [varchar](11) NOT NULL,
	[CustomerID] [varchar](11) NOT NULL,
	[OrderDate] [datetime] NULL,
	[ShipDate] [datetime] NULL,
	[Status] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[OrderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderDetail]    Script Date: 11/22/2022 8:05:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderDetail](
	[OrderID] [varchar](11) NOT NULL,
	[ProductID] [varchar](11) NOT NULL,
	[ColorID] [varchar](11) NOT NULL,
	[OptionID] [varchar](11) NOT NULL,
	[SellPrice] [money] NULL,
	[Quantity] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[OrderID] ASC,
	[ProductID] ASC,
	[ColorID] ASC,
	[OptionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Position]    Script Date: 11/22/2022 8:05:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Position](
	[PositionID] [varchar](11) NOT NULL,
	[DisplayName] [nvarchar](50) NULL,
	[CreatedAt] [datetime] NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[UpdateAt] [datetime] NULL,
	[UpdateBy] [nvarchar](50) NULL,
	[Status] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[PositionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Product]    Script Date: 11/22/2022 8:05:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product](
	[ProductID] [varchar](11) NOT NULL,
	[CategorizeID] [varchar](11) NOT NULL,
	[ProductDiscountID] [varchar](11) NULL,
	[SupplierID] [varchar](11) NOT NULL,
	[DisplayName] [nvarchar](50) NULL,
	[ImportPrice] [money] NULL,
	[SellPrice] [money] NULL,
	[NumberOfLike] [int] NULL,
	[Rating] [float] NULL,
	[NumberOfRating] [int] NULL,
	[Material] [nvarchar](50) NULL,
	[Weight] [float] NULL,
	[Warranty] [int] NULL,
	[ProgrammableButton] [int] NULL,
	[Size] [float] NULL,
	[Description] [nvarchar](300) NULL,
	[ConectionType] [char](1) NULL,
	[CreatedAt] [datetime] NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[UpdateAt] [datetime] NULL,
	[UpdateBy] [nvarchar](50) NULL,
	[Status] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductDiscount]    Script Date: 11/22/2022 8:05:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductDiscount](
	[ProductDiscountID] [varchar](11) NOT NULL,
	[DisplayName] [nvarchar](50) NULL,
	[DiscountPercent] [float] NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[CreatedAt] [datetime] NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[UpdateAt] [datetime] NULL,
	[UpdateBy] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[ProductDiscountID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Supplier]    Script Date: 11/22/2022 8:05:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Supplier](
	[SupplierID] [varchar](11) NOT NULL,
	[DisplayName] [nvarchar](50) NULL,
	[Email] [varchar](50) NULL,
	[Phone] [varchar](11) NULL,
	[HouseNumber] [varchar](30) NULL,
	[Address] [nvarchar](50) NULL,
	[ContractDate] [datetime] NULL,
	[CreatedAt] [datetime] NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[UpdateAt] [datetime] NULL,
	[UpdateBy] [nvarchar](50) NULL,
	[Status] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[SupplierID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WareHouse]    Script Date: 11/22/2022 8:05:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WareHouse](
	[ProductID] [varchar](11) NOT NULL,
	[ColorID] [varchar](11) NOT NULL,
	[OptionID] [varchar](11) NOT NULL,
	[quantity] [char](1) NULL,
PRIMARY KEY CLUSTERED 
(
	[ProductID] ASC,
	[ColorID] ASC,
	[OptionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Cart]  WITH CHECK ADD FOREIGN KEY([ProductID], [ColorID], [OptionID])
REFERENCES [dbo].[WareHouse] ([ProductID], [ColorID], [OptionID])
GO
ALTER TABLE [dbo].[Cart]  WITH CHECK ADD FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customer] ([CustomerID])
GO
ALTER TABLE [dbo].[CustomerAddress]  WITH CHECK ADD FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customer] ([CustomerID])
GO
ALTER TABLE [dbo].[Employee]  WITH CHECK ADD FOREIGN KEY([PositionID])
REFERENCES [dbo].[Position] ([PositionID])
GO
ALTER TABLE [dbo].[FeedBack]  WITH CHECK ADD FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customer] ([CustomerID])
GO
ALTER TABLE [dbo].[FeedBack]  WITH CHECK ADD FOREIGN KEY([ProductID])
REFERENCES [dbo].[Product] ([ProductID])
GO
ALTER TABLE [dbo].[Image]  WITH CHECK ADD FOREIGN KEY([ProductID])
REFERENCES [dbo].[Product] ([ProductID])
GO
ALTER TABLE [dbo].[Invoice]  WITH CHECK ADD FOREIGN KEY([EmployeeID])
REFERENCES [dbo].[Employee] ([EmployeeID])
GO
ALTER TABLE [dbo].[Invoice]  WITH CHECK ADD FOREIGN KEY([OrderID])
REFERENCES [dbo].[Order] ([OrderID])
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customer] ([CustomerID])
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD FOREIGN KEY([DiscountID])
REFERENCES [dbo].[Discount] ([DiscountID])
GO
ALTER TABLE [dbo].[OrderDetail]  WITH CHECK ADD FOREIGN KEY([OrderID])
REFERENCES [dbo].[Order] ([OrderID])
GO
ALTER TABLE [dbo].[OrderDetail]  WITH CHECK ADD FOREIGN KEY([ProductID], [ColorID], [OptionID])
REFERENCES [dbo].[WareHouse] ([ProductID], [ColorID], [OptionID])
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD FOREIGN KEY([CategorizeID])
REFERENCES [dbo].[Categorize] ([CategorizeID])
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD FOREIGN KEY([ProductDiscountID])
REFERENCES [dbo].[ProductDiscount] ([ProductDiscountID])
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD FOREIGN KEY([SupplierID])
REFERENCES [dbo].[Supplier] ([SupplierID])
GO
ALTER TABLE [dbo].[WareHouse]  WITH CHECK ADD FOREIGN KEY([ColorID])
REFERENCES [dbo].[Color] ([ColorID])
GO
ALTER TABLE [dbo].[WareHouse]  WITH CHECK ADD FOREIGN KEY([OptionID])
REFERENCES [dbo].[Options] ([OptionID])
GO
ALTER TABLE [dbo].[WareHouse]  WITH CHECK ADD FOREIGN KEY([ProductID])
REFERENCES [dbo].[Product] ([ProductID])
GO
USE [master]
GO
ALTER DATABASE [GearShop] SET  READ_WRITE 
GO
select @@VERSION