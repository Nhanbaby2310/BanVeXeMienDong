// Update cart count on page every 2 seconds
function updateCartCount() {
    fetch('/api/cartapi/count')
        .then(response => response.json())
        .then(data => {
            const badge = document.querySelector('.cart-badge');
            if (badge) {
                badge.textContent = data.count;
            }
        })
        .catch(error => console.error('Error fetching cart count:', error));
}

// Update cart count every 2 seconds
setInterval(updateCartCount, 2000);

// Update immediately when page loads
document.addEventListener('DOMContentLoaded', updateCartCount);
