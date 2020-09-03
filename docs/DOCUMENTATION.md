# Documentation
## Instructions
The application is pretty straightforward. No installation required!
 Just open your browser and access the follwing address:
 
 [`https://gittagapp.azurewebsites.net`](https://gittagapp.azurewebsites.net)
 
 Provide Github credentials and sign in.
 
 #### List repositories
 To list both starred and non-starred repositories, just access the **List** page.
 
 #### Add tags
 To add tags to a specific repository, access the **Create Tags** page.
 Once there, select the repository to add a tag to, type in the desired tag name and click **Save**.
 
 #### Edit tags
 To change a tag, access the **Update tags** page. Select the repository, select the tag to be updated, type in the new text and click **Save**.
 
 #### Delete tags
 To delete a tag, access the **Delete tags** page, select the repository and tag to be deleted. Click **Delete** to confirm the operation.
 
 #### Search by tag
 To search for starred repositories by tag, just access the **Search** page, enter the search string and click **Search**.
 
 ## Database
 The relational database used to back this application up is Azure SQL. For local testing, Microsoft SQL Server 2019 running in a Docker container was used.
 Database was designed according to the following specifications:
 
 #### Users
 **Column** | **Type** | **PK/FK**
 ---------- | -------- | ---------
 Id | _bigint_ | PK
 Name | _nvarchar_ | -
 
 #### Tags
 **Column** | **Type** | **Pk/FK**
 ---------- | -------- | ---------
 Id | _bigint_ | PK
 Name | _nvarchar_ | -
 
 #### GitRepos
 **Column** | **Type** | **Pk/FK**
 ---------- | -------- | ---------
 Id | _bigint_ | PK
 Name | _nvarchar_ | -
 Description | _nvarchar_ | -
 HttpUrl | _nvarchar_ | -
 UserId | _bigint_ | FK
 
 #### GitRepoTag
 **Column** | **Type** | **PK/FK**
 ---------- | -------- | ---------
 GitRepoId | _bigint_ | PK
 TagId | _bigint_ | PK