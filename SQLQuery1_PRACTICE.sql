/*
SELECT u.Account_ID, (u.First_Name + ' ' + u.Middle_Name + ' ' + u.Last_Name) AS Full_Name, dw.Amount, dw.TransactionType
FROM USER_TABLE u
INNER JOIN DW_TRANSACTION_TABLE dw ON u.Account_ID = dw.Account_ID WHERE u.Account_ID = 1001006 AND dw.TransactionType = 'W';
*/

