// Chart.js rendering function for analytics charts
window.renderAnalyticsChart = function (canvasId, labelsJson, balanceJson, revenueJson, profitJson, marginJson, costsJson, unitsJson, returnsJson, defectiveJson) {
    console.log(`renderAnalyticsChart called for ${canvasId}`);

    // Check if Chart.js is loaded
    if (typeof Chart === 'undefined') {
        console.error('Chart.js is not loaded!');
        return;
    }

    const ctx = document.getElementById(canvasId);

    if (!ctx) {
        console.error(`Canvas element with id '${canvasId}' not found`);
        return;
    }

    try {
        // Parse JSON data
        const labels = JSON.parse(labelsJson);
        const balance = JSON.parse(balanceJson);
        const revenue = JSON.parse(revenueJson);
        const profit = JSON.parse(profitJson);
        const margin = JSON.parse(marginJson);
        const costs = JSON.parse(costsJson);
        const units = JSON.parse(unitsJson);
        const returns = JSON.parse(returnsJson);
        const defective = JSON.parse(defectiveJson);

        console.log(`Data parsed for ${canvasId}: ${labels.length} items`);
        console.log('Labels:', labels);
        console.log('Balance:', balance);
        console.log('Revenue:', revenue);

    // Destroy existing chart if it exists
    if (window[`chart_${canvasId}`]) {
        window[`chart_${canvasId}`].destroy();
    }

    // Create new chart
    window[`chart_${canvasId}`] = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: labels,
            datasets: [
                {
                    label: 'Сальдо (₽)',
                    data: balance,
                    backgroundColor: 'rgba(214, 51, 132, 0.7)',
                    borderColor: 'rgba(214, 51, 132, 1)',
                    borderWidth: 1,
                    yAxisID: 'y'
                },
                {
                    label: 'Выручка (₽)',
                    data: revenue,
                    backgroundColor: 'rgba(25, 135, 84, 0.7)',
                    borderColor: 'rgba(25, 135, 84, 1)',
                    borderWidth: 1,
                    yAxisID: 'y'
                },
                {
                    label: 'Прибыль (₽)',
                    data: profit,
                    backgroundColor: 'rgba(13, 110, 253, 0.7)',
                    borderColor: 'rgba(13, 110, 253, 1)',
                    borderWidth: 1,
                    yAxisID: 'y'
                },
                {
                    label: 'Маржа (%)',
                    data: margin,
                    backgroundColor: 'rgba(102, 16, 242, 0.7)',
                    borderColor: 'rgba(102, 16, 242, 1)',
                    borderWidth: 1,
                    yAxisID: 'y1',
                    type: 'line'
                },
                {
                    label: 'Затраты (₽)',
                    data: costs,
                    backgroundColor: 'rgba(255, 193, 7, 0.7)',
                    borderColor: 'rgba(255, 193, 7, 1)',
                    borderWidth: 1,
                    yAxisID: 'y'
                },
                {
                    label: 'Штук',
                    data: units,
                    backgroundColor: 'rgba(13, 202, 240, 0.7)',
                    borderColor: 'rgba(13, 202, 240, 1)',
                    borderWidth: 1,
                    yAxisID: 'y2'
                },
                {
                    label: 'Возвраты',
                    data: returns,
                    backgroundColor: 'rgba(220, 53, 69, 0.7)',
                    borderColor: 'rgba(220, 53, 69, 1)',
                    borderWidth: 1,
                    yAxisID: 'y2'
                },
                {
                    label: 'Брак',
                    data: defective,
                    backgroundColor: 'rgba(108, 117, 125, 0.7)',
                    borderColor: 'rgba(108, 117, 125, 1)',
                    borderWidth: 1,
                    yAxisID: 'y2'
                }
            ]
        },
        options: {
            responsive: true,
            maintainAspectRatio: true,
            interaction: {
                mode: 'index',
                intersect: false,
            },
            plugins: {
                legend: {
                    position: 'top',
                    labels: {
                        usePointStyle: true,
                        padding: 15,
                        font: {
                            size: 11
                        }
                    }
                },
                tooltip: {
                    callbacks: {
                        label: function(context) {
                            let label = context.dataset.label || '';
                            if (label) {
                                label += ': ';
                            }
                            if (context.dataset.yAxisID === 'y1') {
                                // Маржа в процентах
                                label += context.parsed.y.toFixed(1) + '%';
                            } else if (context.dataset.yAxisID === 'y2') {
                                // Штуки, возвраты, брак
                                label += context.parsed.y + ' шт.';
                            } else {
                                // Денежные значения
                                label += new Intl.NumberFormat('ru-RU', {
                                    style: 'currency',
                                    currency: 'RUB',
                                    minimumFractionDigits: 0,
                                    maximumFractionDigits: 0
                                }).format(context.parsed.y);
                            }
                            return label;
                        }
                    }
                }
            },
            scales: {
                x: {
                    grid: {
                        display: false
                    }
                },
                y: {
                    type: 'linear',
                    display: true,
                    position: 'left',
                    title: {
                        display: true,
                        text: 'Денежные значения (₽)'
                    },
                    ticks: {
                        callback: function(value) {
                            return new Intl.NumberFormat('ru-RU', {
                                style: 'currency',
                                currency: 'RUB',
                                minimumFractionDigits: 0,
                                maximumFractionDigits: 0
                            }).format(value);
                        }
                    }
                },
                y1: {
                    type: 'linear',
                    display: true,
                    position: 'right',
                    title: {
                        display: true,
                        text: 'Маржа (%)'
                    },
                    grid: {
                        drawOnChartArea: false,
                    },
                    ticks: {
                        callback: function(value) {
                            return value.toFixed(1) + '%';
                        }
                    }
                },
                y2: {
                    type: 'linear',
                    display: false,
                    position: 'right',
                    grid: {
                        drawOnChartArea: false,
                    }
                }
            }
        }
    });

        console.log(`Chart created successfully for ${canvasId}`);
    } catch (error) {
        console.error(`Error rendering chart ${canvasId}:`, error);
    }
};
