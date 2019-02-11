SELECT ds.Id, ds.Name, ds.AcquisitionDate, ds.Data, a.AttributeName
FROM DataSeries as ds
INNER JOIN DataSeries_Attributes as da ON ds.Id = da.DataSeries_Id
INNER JOIN Attributes as a ON a.AttributeName = da.Attributes_AttributeName