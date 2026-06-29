Use EWalletDB;


CREATE TABLE USER_TABLE (
	Account_ID INT PRIMARY KEY IDENTITY(1000000, 1),
	First_Name VARCHAR(30) NOT NULL,
	Middle_Name VARCHAR(30),
	Last_Name VARCHAR(30) NOT NULL,
	Email VARCHAR(50) NOT NULL,
	PhoneNumber VARCHAR(15) NOT NULL,
	Date_Registered DATE NOT NULL,
	Total_Current_Balance DECIMAL(10,2) NOT NULL,
	Username VARCHAR(30) NOT NULL,
	userPassword VARCHAR(30) NOT NULL,

	CONSTRAINT chk_balance CHECK (Total_Current_Balance >= 0 AND Total_Current_Balance <= 10000),

	CONSTRAINT chk_Account_ID CHECK (Account_ID <= 9999999 AND Account_ID >= 1000000)

);











--CREATE TABLE USER_TABLE (
	--Account_ID INT PRIMARY KEY IDENTITY(1000000, 1),
	--First_Name VARCHAR(30) NOT NULL,
	--Middle_Name VARCHAR(30),
	--Last_Name VARCHAR(30) NOT NULL,
	--Email VARCHAR(50) NOT NULL,
	--PhoneNumber VARCHAR(15) NOT NULL,
	--Date_Registered DATE NOT NULL,
	--Total_Current_Balance DECIMAL(10,2) NOT NULL,
	--Username VARCHAR(30) NOT NULL,
	--userPassword VARCHAR(30) NOT NULL,

--	CONSTRAINT chk_balance CHECK (Total_Current_Balance >= 0 AND Total_Current_Balance <= 10000),

--	CONSTRAINT chk_Account_ID CHECK (Account_ID <= 9999999 AND Account_ID >= 1000000)

--);

