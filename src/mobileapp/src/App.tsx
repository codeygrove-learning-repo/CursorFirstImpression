import { useState } from 'react';

function App() {
  const [orderId, setOrderId] = useState('');
  const [status, setStatus] = useState<string | null>(null);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setStatus(null);
    if (!orderId) {
      setStatus('Please enter an order number.');
      return;
    }
    try {
      // Adjust the API URL as needed for your environment
      const response = await fetch(`https://localhost:7048/api/order/deliver/${orderId}`, {
        method: 'PUT',
      });
      if (response.ok) {
        setStatus('Order marked as delivered!');
      } else {
        setStatus('Failed to mark order as delivered.');
      }
    } catch (err) {
      setStatus('Error connecting to API.');
    }
  };

  return (
    <div style={{ maxWidth: 400, margin: '2rem auto', padding: 24, border: '1px solid #ccc', borderRadius: 8 }}>
      <h2>Order Delivery Confirmation</h2>
      <form onSubmit={handleSubmit}>
        <label htmlFor="orderId">Order Number:</label>
        <input
          id="orderId"
          type="text"
          value={orderId}
          onChange={e => setOrderId(e.target.value)}
          style={{ width: '100%', marginBottom: 12, padding: 8 }}
        />
        <button type="submit" style={{ width: '100%', padding: 10, background: '#007bff', color: '#fff', border: 'none', borderRadius: 4 }}>
          Order Delivered
        </button>
      </form>
      {status && <div style={{ marginTop: 16, color: status.includes('Error') || status.includes('Failed') ? 'red' : 'green' }}>{status}</div>}
    </div>
  );
}

export default App;
