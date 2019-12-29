// Set new default font family and font color to mimic Bootstrap's default styling
Chart.defaults.global.defaultFontFamily = 'Nunito', '-apple-system,system-ui,BlinkMacSystemFont,"Segoe UI",Roboto,"Helvetica Neue",Arial,sans-serif';
Chart.defaults.global.defaultFontColor = '#858796';
var sp1 = document.getElementById("sp1");
var sp2 = document.getElementById("sp2");
var sp3 = document.getElementById("sp3");
var sp4 = document.getElementById("sp4");
// Pie Chart Example
var ctx = document.getElementById("myPieChart");
var myPieChart = new Chart(ctx, {
  type: 'doughnut',
  data: {
    labels: [sp1.className, sp2.className,sp3.className,"Sản Phẩm Khác"],
    datasets: [{
      // thay doi ở đây cũng là thay đổi thông số ở trên biểu đồ,và biểu đồ sẽ thay đổi theo các số trên đây tất cả cộng lại phải =100
      data: [sp1.value,sp2.value,sp3.value,sp4.value],
      backgroundColor: ['#4e73df', '#1cc88a', '#36b9cc','#E74A3B'],
      hoverBackgroundColor: ['#2e59d9', '#17a673', '#2c9faf','#CF2000'],
      hoverBorderColor: "rgba(234, 236, 244, 1)",
    }],
  },
  options: {
    maintainAspectRatio: false,
    tooltips: {
      backgroundColor: "rgb(255,255,255)",
      bodyFontColor: "#858796",
      borderColor: '#dddfeb',
      borderWidth: 1,
      xPadding: 15,
      yPadding: 15,
      displayColors: false,
      caretPadding: 10,
    },
    legend: {
      display: false
    },
    cutoutPercentage: 80,
  },
});
