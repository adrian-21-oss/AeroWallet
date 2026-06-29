Use EWalletDB;

CREATE TABLE SR_TRANSACTION_TABLE(
	SR_Transaction_ID INT PRIMARY KEY,
	Transaction_Date DATE NOT NULL,
	Transaction_Time TIME NOT NULL,
	Amount DECIMAL(10,2) NOT NULL,
	Account_SendTo INT NOT NULL,
	Account_ReceiveFrom INT NOT NULL,


	CONSTRAINT chk_SR_Amount CHECK (Amount >= 100 AND Amount <= 2000),

	CONSTRAINT chk_SR_Transaction_ID CHECK (SR_Transaction_ID BETWEEN 1000000 AND 9999999),

	CONSTRAINT fk_Account_SendTo FOREIGN KEY (Account_SendTo) REFERENCES USER_TABLE(Account_ID),

	CONSTRAINT fk_Account_ReceiveFrom FOREIGN KEY (Account_ReceiveFrom) REFERENCES USER_TABLE(Account_ID)
	
);