CREATE function [dbo].[GetDocumentAndRequestElements](@Id_RequestType int, @Id_LicenseRequest int)
returns @T table(DocumentStateName nvarchar(max)
      ,RequestElementTypeId int
      ,RequestElementTypeName nvarchar(max) 
      ,[Id] int
      ,[Id_Request] int
      ,[Id_Document] int
      ,[Id_DocumentState] int
      ,[Id_RequestElement] nvarchar(max)
	  ,[Id_ParentRequestElement] nvarchar(max)
	  ,[IsActive] bit
      ,[Description]nvarchar(max)
      ,[HasDocument] bit
      ,[RowNum] int ,primary key (RowNum)) 
as
begin

 with tree as (
    SELECT        
        Id, 
        Id_Parent,
		IsActive,
        0 AS LEVEL
    FROM RequestElement
    WHERE Id_Parent is null
  
    UNION ALL
  
    SELECT        
        rn.Id, 
        rn.Id_Parent, 
		rn.IsActive,
        LEVEL + 1
    FROM            RequestElement rn 
    INNER JOIN    tree ON tree.Id = rn.Id_Parent
  )

insert into @T


  select 
  ds.Name_ru AS DocumentStateName, ret.Id AS RequestElementTypeId, ret.Name_ru AS RequestElementTypeName, dir.Id,
dir.Id_Request as Id_Request, dir.Id_Document, 
                         dir.Id_DocumentState, case 
      when dir.Id is null then cast (re.Id as nvarchar(max))
      when dir.Id is not null then cast (re.Id as nvarchar(max)) + '_' + cast (dir.Id as nvarchar(max))
    end as Id_RequestElement, re.Id_Parent as Id_ParentRequestElement,
					re.IsActive,	 dir.Description, cast(CASE WHEN dir.Id IS NULL THEN 0 ELSE 1 END as bit)AS HasDocument, RowNum = ROW_NUMBER() OVER (ORDER BY dir.Id)
  from tree t
  inner join RequestElement re on re.Id = t.Id
  left join DocumentInRequest dir on dir.Id_RequestElement = t.Id  and dir.Id_Request = @Id_LicenseRequest
  LEFT OUTER JOIN
                         dbo.RequestElementType AS ret ON ret.Id = re.Id_RequestElemType LEFT OUTER JOIN
                         dbo.DocumentState AS ds ON ds.Id = dir.Id_DocumentState
WHERE        (re.Id_RequestType = @Id_RequestType) 


  return
end