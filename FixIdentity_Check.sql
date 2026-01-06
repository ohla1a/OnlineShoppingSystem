-- Check Orders identity info and current value
SELECT 
  OBJECT_NAME(object_id) AS TableName,
  name AS ColumnName,
  COLUMNPROPERTY(object_id('dbo.Orders'), 'OrderID', 'IsIdentity') AS IsIdentity,
  IDENT_CURRENT('dbo.Orders') AS CurrentIdent,
  (SELECT COUNT(*) FROM dbo.Orders) AS RowCount,
  (SELECT ISNULL(MAX(OrderID),0) FROM dbo.Orders) AS MaxOrderID;

-- List triggers on Orders (if any)
SELECT name FROM sys.triggers WHERE parent_id = OBJECT_ID('dbo.Orders');