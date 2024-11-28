# CrypTo

### CrypTo is a robust blockchain-based application built with C#, SQL, and RabbitMQ. This project provides a fully functional API that enables:

    Wallet Management: Create and manage wallets seamlessly.
    Transactions: Execute secure transactions between wallets.
    Blockchain Mining: Mine blocks to validate and persist pending transactions into the blockchain.

### Key Features:

    Pending Transaction Queue: Transactions are queued in RabbitMQ for processing as "Pending Transactions."
    Blockchain Integration: Transactions are added to the blockchain after a block is mined.
    SQL Database: Persistent storage for wallets, transactions, and blockchain data.
    Event-Driven Architecture: Utilizes RabbitMQ to decouple transaction creation from blockchain processing.

### This project is designed for scalability and modularity, showcasing best practices in distributed systems and blockchain technology.
