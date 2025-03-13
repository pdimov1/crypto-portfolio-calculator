const Portfolio = (function () {
    const DEFAULT_REFRESH_INTERVAL = 300000; // 5 minutes in milliseconds

    async function init() {
        try {
            const refreshInterval = await getRefreshInterval();

            initializePortfolioData();

            setInterval(refreshPortfolio, refreshInterval);

        } catch (error) {
            console.error('Error initializing portfolio refresher:', error);
            updateStatusMessage('Failed to initialize portfolio refresh');
        }
    }

    function initializePortfolioData() {
        const portfolioItems = JSON.parse(sessionStorage.getItem('portfolioItems')) || [];

        if (portfolioItems.length === 0) {
            const modelData = document.getElementById("portfolio-data").textContent;
            sessionStorage.setItem('portfolioItems', modelData);
        }
    }

    async function getRefreshInterval() {
        try {
            const response = await fetch('/refresh-interval');
            const data = await response.json();
            return data.interval || DEFAULT_REFRESH_INTERVAL;
        } catch (error) {
            console.error('Error fetching refresh interval:', error);
            return DEFAULT_REFRESH_INTERVAL;
        }
    }

    async function refreshPortfolio() {
        const portfolioItems = JSON.parse(sessionStorage.getItem('portfolioItems'));

        if (!portfolioItems || portfolioItems.length === 0) {
            document.getElementById('refresh-status').textContent = 'Error: No portfolio data found';
            return;
        }

        const token = document.querySelector('input[name="__RequestVerificationToken"]').value;

        try {
            const response = await fetch('/portfolio-refresh', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'X-CSRF-TOKEN': token
                },
                body: JSON.stringify(portfolioItems)
            });

            if (response.ok) {
                const html = await response.text();
                const portfolioContainer = document.getElementById('portfolioContainer');
                if (portfolioContainer) {
                    portfolioContainer.innerHTML = html;
                    updateStatusMessage('Portfolio updated successfully', true);
                }
            }

        } catch (e) {
            console.error('Error refreshing portfolio:', error);
            updateStatusMessage(`Error updating portfolio: ${error.message}`);
        }

        function updateStatusMessage(message, autoHide = false) {
            const statusElement = document.getElementById('refresh-status');
            if (statusElement) {
                statusElement.textContent = message;

                if (autoHide) {
                    setTimeout(() => {
                        statusElement.textContent = '';
                    }, 3000);
                }
            }
        }
    }

    return {
        init,
        refreshPortfolio
    };

})();

document.addEventListener('DOMContentLoaded', Portfolio.init);