# CursorFirstImpression
First impression using Cursor AI Agent

# Pre-requisite
1. You have free mongodb cluster (for my test, database name is called: "AspireLearning" with two collections: catalogs and orders)
2. You have created a service bus broker (for my test i used Azure) - queue name: orders
3. You have created an event hub broker (for my test i used Azure) - event hub name: order-delivered with three consumer group: replenishment, vendor and invoice
4. .Net 9.0

# Update Connection String
Replace the following connection string:
1. <MONGDB_CONNECTION_STRING>
2. <EVENTHUB_CONNECTION_STRING>
3. <SERVICEBUS_CONNECTION_STRING>

