# BankManagerApp

## 📃Description
* BankManagerApp is a .NET project that provides a simulated bank administration site experience on a smaller scale. It contains 4 projects that all relates to the same fictional bank managing scandinavian countrys. The main project called "BankManagerApp" is a C# Razor pages app that enables users to enter a functional website and perform various operations such as viewing top customers by country, all customers and accounts, as well as conducting transactions. The main application is intended for bank employees and thus targets the administrative user experience. Users can log in as two different roles (Admin or Cashier) and perform various actions based on their logged in credentials. Cashiers handles everything associated with customers, accounts and transactions, including registering new customers or editing existing ones. Admins are responsible for managing all registered users and can create new users(Admins/Cashiers) or edit/delete existing ones. The application is built of an existing database which contains a large quantity of pre-existing customers, accounts and transactions. Every action, such as conducting transactions or managing customers/users is registered to this database.        

* "BankWebAPI" is a an ASP.NET Core Web API included in the BankManagerApp solution. It's intended purpose is to simulate an API associated provided by the bank with the intention of providing customers with the ability to log in and view their customer and transaction information. The API retrieves all customer information at launch and provides them with log in information based on their customer id. Through an API-interface such as swagger or postman, it is possible to log in as a customer and view the relevant data. If a customer tries to access someone elses information through an HTTP request it will recieve the 401 Unauthorized error code. There is also an admin log in that has the ability to authorization to view all customer and account data.      

* "AntiMoneyLaundering" is a C# console application included in the BankManagerApp solution. The application simulates a money laundering checker that monitors all transactions and sorts those suspected of money laundering and prints these to a .txt file. The program also keeps track of the last monitoring date and only adds the most recent transactions based of this date. 

* "BankRepository" is a class library shared by all applications in the solution. It contains database entities, services, viewmodels and infrastructure classes needed for communicating with the database ande performing crud operations on customers and users.

## :computer: Usage
* "BankManagerApp": The application will start at the open Index page where users can view country statistics and navigate to the top customers page. If users want to access further information or perform actions, a log in is required. There are two main log in accounts seeded at the start of the application:


  E-mail: richard.chalk@systementor.se
  
  pw: Hejsan123#
  
  role: Admin 
 
  E-mail: richard.erdos.chalk@gmail.se 
  
  pw: Hejsan123# 
  
  role: Cashier

  After log in, users can start performing various actions based on their logged in user role credentials. Note that Cashiers can't view Admin pages and vice versa, so remember to switch accounts to be able to perform     different actions!   

* "BankWebAPI": To access the customer & transaction information through the API, customers have to log in. In this application, customer log in credentials are simulated att runtime using the unique customerid's registered in the database. An example log in credential for the customer with Id: 1 looks like this: 

  UserName: user_1   
  pw: 1

  If you want to log in as another customer you replace the 1 with that customers id represented in the database. There is also an admin login that is fully authorized. The log in credentials for the admin is:
  
     UserName = richard_admin
     
     pw = passwordAdmin
     
     There are three possible endpoint calls to the API: 
      
     POST/api/Login
     
     GET/api/Me/{id}
     
     GET/api/Account/{id}/{limit}/{offset}
     
     Use the POST/api/Login endpoint to receive the necessary Bearer Token that is needed to become authorized.
     
     Use the GET/api/Me/{id} endpoint to view a customers full information. Replace {id} with relevant customer id, for exampel 1.
     
     Use the GET/api/Account/{id}/{limit}/{offset} endpoint to view a customers transactions and specify the desired range. The limit parameter determines the number of transactions to be displayed, while the offset parameter decides the starting point in the transaction list. By adjusting the offset, you can skip a certain number of records to retrieve transactions beyond the starting range.
     
 * "AntiMoneyLaundering": The console app will automatically check for suspected transaction. The rules for suspected transactions is:
 
    1. An individual transaction greater than 15,000 €.
    
    2. The total transactions in the last three days (72 hours) from the current time are greater than 23,000 €.


    The application will automatically create a folder called "MonitoringData" that will contain the results of the transaction monitoring in the form of .txt files that are registered per country and date of monitoring. 

    
    




