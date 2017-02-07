/// <reference path="../Scripts/knockout-3.1.0.js" />
/// <reference path="../Scripts/jquery-1.10.2.js" />

var studentRegisterViewModel;

// use as register student views view model
function Student(price) {
    var self = this;

    // observable are update elements upon changes, also update on element data changes [two way binding]
    self.janeshin = ko.observable(1);
    self.kir = ko.observable(price)

    // create computed field by combining first name and last name
    self.ziba = ko.computed(function () {
        return self.janeshin() * self.kir();
    }, self);


        // remove computed field from JSON data which server is not expecting
        delete dataObject.ziba;

        //$.ajax({
        //    url: '/api/student',
        //    type: 'post',
        //    data: dataObject,
        //    contentType: 'application/json',
        //    success: function (data) {
        //        studentRegisterViewModel.studentListViewModel.students.push(new Student(data.Id, data.FirstName, data.LastName, data.Age, data.Description, data.Gender));

        //        self.Id(null);
        //        self.FirstName('');
        //        self.LastName('');
        //        self.Age('');
        //        self.Description('');
        //    }
        //});

} // end of product



// create index view view model which contain two models for partial views
studentRegisterViewModel = { addStudentViewModel: new Student() };

// on document ready
$(document).ready(function () {
    // bind view model to referring view
    ko.applyBindings(studentRegisterViewModel);

    // load student data
    studentRegisterViewModel.studentListViewModel.getStudents();
});