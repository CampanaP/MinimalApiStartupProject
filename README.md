# MinimalApiStartupProject
Template for visual studio, to be used to create modular WebApi quickly and easily.  
Project running on first start-up, only needs to be linked to the DB.  
Dapper was used, it was configured to be used with SQL Server DB.    
### Instructions for use:  
 - Clone this project
 - Open **Visual Studio 2022**
 - Open **MinimalApiStartupProject**
 - In **Visual Studio 2022** in the **Project** menu, select **Export Template**
 - In **Export Template Wizard** choose **Choose Template Type** as **Project Template**
 - In **Export Template Wizard** select **Finish**
 - Close **Visual Studio 2022**
 - Go to the folder where the template .zip was generated and copy it to: **C:\Users\%USERPROFILE%\Documents\Visual Studio 2022\Templates\ProjectTemplates\C#**
 - Open **Visual Studio 2022**
 - Click: **Create a new project**
 - Select **MinimalApiStartupProject** as a Template
 - Set the fields: **Project name**, **Location** and **Solution name** with the values you need
 - Select **Create** 
 - Start working and good luck

### Sources:  
 - https://timdeschryver.dev/blog/maybe-its-time-to-rethink-our-project-structure-with-dot-net-6
 - https://learn.microsoft.com/en-us/visualstudio/ide/how-to-create-project-templates?view=vs-2022
