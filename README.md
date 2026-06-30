# **MicroserviceManager** 

### **A simple tool for managing microservice architecture in a single terminal**

This simple tool consolidates a multi-microservice architecture into a **single terminal**, giving you **full control over the workflow and enabling you to quickly identify all errors and issues.** All you need to do is enter the paths to the microservices' executable files and logs into the database, and specify their dependencies on one another. 



### List of commands available in the app right now:
create name path log_path (optional) dependence1;dependence2 (optional)           - Add a new microservice to the database

remove name                                                                       - Delete microservice from the database

edit name new_path new_log_path (optional) new_dependence1;dependence2 (optional) - modify information about an existing microservice in the database

run name                                                                          - Launch the microservice (if possible)

stop name                                                                         -  Stop the microservice (if possible)

stats name                                                                        - Get information about the status and load of a microservice

errors name                                                                       - Find information about operational errors in the microservice logs

problems-analysis                                                                 - Conduct a detailed analysis of all possible causes of issues affecting the operation of the entire architecture and each individual microservice



## **User Guide**
1. Download the archive containing the ready-to-use application, or the source code from the latest release.
2. Ensure that .NET is installed on your device.
3. If you have selected the archive, it already contains all the files necessary for operation, and you can start using it at this stage.
4. **If you are using the source code**, you need to create a database file upon the first launch; to do this, create a 'Database' folder in the application directory before running the program, and uncomment the database file creation method in the `Program.cs` class.



## SUPPORT THIS PROJECT
If MicroserviceManager helps you in your work, please star the repository on GitHub. It motivates us to keep developing the project!
