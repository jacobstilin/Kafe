# Kafe
Jacob Stilin Solo Capstone

This is my final project that I did while attending Milwaukee devCodeCamp.

Kafe Cruisers is a mobile responsive web application created used ASP.NET MVC5, SQL, Google Maps and Stripe APIs, jQuery, HTML5 and CSS3. It is a mobile ordering system for a company I created in an entrepreneurship course at the University of Wisconsin Milwaukee. 

The Kafe Cruisers business is a fleet of coffee trucks that use mobile only sales to get coffee to customers efficiently and on time. The company is unique in that there are no face to face sales, eliminating the need for cashier employees, and customers are required to schedule a pick up time for their coffee, meaning there are never any lines or waiting. Available times are restricted based on current customer demand. 

This application is not hosted online. I have posted screenshots so you can get an idea for what the project is like. Feel free to download and run the application on your device. 

This application has two distinct sides for the customers and employees. The employee side is tailored for use on iPads and similar tablets. Employees mainly use the order page. Here they can view each item along with its specific ingredients and instructions for each order and set them as filled when complete. Employees also manage the trucks and menus for each truck. Each truck is assigned to a specific location for the day. Employees are able to add new locations and assign trucks to them as needed. Truck hours can also be modified. Truck menus are easily modified as well. Employees can add and remove menu items in seconds. 

On the customer side, customers are able to place orders, view current and past orders and resume the last order that has yet to be completed or filled. The customer side is tailored for iPhone and similar mobile devices. When starting an order, customers use the Google Maps API to select what truck they wish to order from. They then select a drink to order along with any specifications. They can add as many drinks as they wish to order before proceeding to checkout. They are then directed to schedule a pick up time. Pick up times are restricted by the ammount of drinks in the order and how long they take to make, so orders can never be scheduled too close to the current time. The time picker also accounts for the truck hours. Times when customer demand is too high are greyed-out and can not be selected. After scheduling a time, the customer uses the Stripe API to pay with a credit card. They can use one on file or set up a new card. After placing the order they are given a unique ID number to show to a barista.

Customers are able to view a list of current orders that have not been completed or picked up yet. At any time they can resume these orders or cancel them. They can also view a list of past orders and all relevant information. If need be they can simply select Resume Order and continue on with the last order they were working on. 

This application was created by and is maintained by Jacob Stilin. Feel free to contact me with any questions or comments!

/Images/KCipadLogin.PNG
