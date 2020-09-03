# GitTagApp
![License](GitTagApp/wwwroot/images/license-MIT.svg)
![Version](GitTagApp/wwwroot/images/version-v1.0.2.svg)
## Objective
Create a WebApp to help users keep track of starred Github repositories.
This app allows the user to add custom tags to starred repositories, edit, delete and search by those tags.
The whole idea is to make Github user's life a little easier about finding starred repositories.

This project's architecture includes:

## Backend - API
 - Add authentication and authorization
 - Add CRUD functionality to the application
 - Fetch all repositories starred by the user
 - Add endpoints to be accessed by the frontend to fetch and retrieve tag information for each starred repository
 ## Frontend
 - Provide a user interface for authentication
 - Provide access to the endpoints created by the backend
 - Interface should be accessed by both desktop or mobile browsers
 ## Technologies used
 This application was created or makes use of the following technologies:
  + .NET Core
    + Razor Pages
    + xUnit
  + Oauth 2.0
  + Github
  + Azure:
    + Key Vault
    + App Service
    + DevOps
    + Azure SQL
  
 ## How to use
 The application is pretty straightforward. No installation required!
 Just open your browser and access the follwing address:
 
 [`<https://gittagapp.azurewebsites.net>`](https://gittagapp.azurewebsites.net)
 
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