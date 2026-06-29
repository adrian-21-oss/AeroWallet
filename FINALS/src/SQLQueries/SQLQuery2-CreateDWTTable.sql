Use EWalletDB;

CREATE TABLE DW_TRANSACTION_TABLE(
	DW_Transaction_ID INT PRIMARY KEY,
	Account_ID INT NOT NULL,
	TransactionType VARCHAR(20) NOT NULL,
	Amount DECIMAL(10,2) NOT NULL,
	
	TransactionDate DATE NOT NULL,
	TransactionTime TIME NOT NULL,


	CONSTRAINT chk_DW_Amount CHECK (Amount >= 100 AND Amount <= 2000),

	CONSTRAINT chk_TransactionType CHECK (TransactionType IN ('D', 'W')),

	CONSTRAINT chk_DW_Transaction_ID CHECK (DW_Transaction_ID BETWEEN 1000000 AND 9999999),

	CONSTRAINT fk_Account_ID FOREIGN KEY (Account_ID) REFERENCES USER_TABLE(Account_ID)
);
