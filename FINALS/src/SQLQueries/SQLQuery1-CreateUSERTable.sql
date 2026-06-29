Use EWalletDB;

CREATE TABLE USER_TABLE (
	Account_ID INT PRIMARY KEY,
	First_Name VARCHAR(30) NOT NULL,
	Middle_Name VARCHAR(30),
	Last_Name VARCHAR(30) NOT NULL,
	Date_Registered DATE NOT NULL,
	Total_Current_Balance DECIMAL(10,2) NOT NULL,
	userPassword VARCHAR(30),

	CONSTRAINT chk_balance CHECK (Total_Current_Balance >= 0 AND Total_Current_Balance <= 10000)
);

ALTER TABLE USER_TABLE
ADD CONSTRAINT chk_Account_ID CHECK (Account_ID <= 9999999 AND Account_ID >= 1000000)