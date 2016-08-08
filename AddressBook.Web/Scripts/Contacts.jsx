var ContactBox = React.createClass({
    loadContactsFromServer: function () {
        $.ajax({
            url: this.props.url,
            dataType: 'json',
            cache: false,
            success: function (data) {
                this.setState({ data: JSON.parse(data) });
            }.bind(this),
            error: function (xhr, status, err) {
                console.error(this.props.url, status, err.toString());
            }.bind(this)
        });
    },

    handleContactSubmit: function (contact) {
        $.ajax({
            url: this.props.url,
            dataType: 'json',
            type: 'POST',
            data: contact,
            success: function (data) {
                this.setState({ data: data });
            }.bind(this),
            error: function (xhr, status, err) {
                console.error(this.props.url, status, err.toString());
            }.bind(this)
        });
    },

    getInitialState: function () {
        return { data: [] };
    },

    componentDidMount: function () {
        this.loadContactsFromServer();
        setInterval(this.loadContactsFromServer, this.props.pollInterval);
    },

    render: function () {
        return (
            <div className="contactBox">
                <h2 className="col-md-12">Contacts</h2>

                {/* ToDo: Add a header row. How? not sure... */} 
                <ContactList data={this.state.data} />

                <ContactForm onContactSubmit={this.handleContactSubmit} />
            </div>
        )
    }
});

var ContactList = React.createClass({
    render: function () {
        var contactNodes = this.props.data.map(function (contact) {
            return (
                <Contact key={contact.Id}
                            id={contact.Id}
                            firstName={contact.FirstName}
                            lastName={contact.LastName}
                            nickname={contact.Nickname}
                            email={contact.Email}
                            phoneNumbers={contact.PhoneNumbers} />
            );
        });
        var rowNumber = 0;
        return (
            <div className="contactList container">
                {contactNodes}
            </div>
        );
    }
});

// TODO: Get rid of the error below (not sure how though)
// Warning: Each child in an array or iterator should have a unique "key" prop. Check the render method of `Contact`.
var Contact = React.createClass({
    handleDelete: function (e) {
        alert("delete contact " + GetNumeric(e.target.id) + "; not implemented");
        return false;
    },

    handleEdit: function (e) {
        alert("edit contact " + GetNumeric(e.target.id) + "; not implemented");
        return false;
    },

    render: function () {
        var phoneNodes = this.props.phoneNumbers.map(function (phoneNumber) {
            return (
                <PhoneNumber id={phoneNumber.Id}
                             contactId={phoneNumber.ContactId}
                             phoneNumber={phoneNumber.Number} />
            );
        });

        return (
            <div className="row contact" key={this.props.id} id={"contact" + this.props.id}>
                <div className="col-md-3">{this.props.firstName} {this.props.lastName}</div>
                <div className="col-md-2">{this.props.nickname}</div>
                <div className="col-md-3">{this.props.email}</div>
                <div className="col-md-2">
                    {phoneNodes}
                    <a href="#" className="">Add</a>
                </div>
                <div className="col-md-2 text-right">
                    <a href="#" className="btn btn-xs btn-default contactButton" 
                       id={"Edit" + this.props.id} data-id={this.props.id} onClick={this.handleEdit}>Edit</a>

                    <a href="#" className="btn btn-xs btn-warning contactButton" 
                       id={"Delete" + this.props.id} data-id={this.props.id} onClick={this.handleDelete}>Delete</a>
                </div>
            </div>

        );
    }
});

var PhoneNumber = React.createClass({
    render: function () {
        var number = this.props.phoneNumber;
        var formattedNumber = number.substr(0, 3) + '.' + number.substr(3, 3) + '.' + number.substr(6, 4)
        return (
            <div><span>{formattedNumber}</span></div>
        );
    }
});

var GetNumeric = function (value) {
    return value.match(/\d+/g);
}

var ContactForm = React.createClass({
    getInitialState: function () {
        return { firstName: '', lastName: '', nickname: '', email: '', phoneNumber: '' };
    },
    handleFirstNameChange: function (e) {
        $("#firstNameError").empty();
        this.setState({ firstName: e.target.value });
    },
    handleLastNameChange: function (e) {
        this.setState({ lastName: e.target.value });
    },
    handleNicknameChange: function (e) {
        this.setState({ nickname: e.target.value });
    },
    handleEmailChange: function (e) {
        this.setState({ email: e.target.value });
    },
    handlePhoneNumberChange: function (e) {
        $("phoneNumberError").empty();
        this.setState({ phoneNumber: e.target.value });
    },

    handleSubmit: function (e) {
        e.preventDefault();
        var firstName = this.state.firstName.trim();
        var lastName = this.state.lastName.trim();
        var nickname = this.state.nickname.trim();
        var email = this.state.email.trim();
        var phoneNumber = this.state.phoneNumber.trim();

        var errorsFound = false;
        // ToDo: refactor validation out into a separate function
        if (!firstName) {
            $("#firstNameError").text("First Name is required!")
            errorsFound = true;
        }
        if (firstName === "Chad" || firstName === "chad") {
            $("#firstNameError").text("I'm sorry, Chad is not an acceptable name.")
            errorsFound = true;
        }

        // ToDo: use google's E.164 phone number validation library https://github.com/googlei18n/libphonenumber
        var numeric = GetNumeric(phoneNumber);
        if (phoneNumber && !numeric) {
            $("#phoneNumberError").text("Phone numbers must include actual numbers.");
            errorsFound = true;
        } else if (numeric && (numeric.length < 8 || numeric.length > 15)) {
            $("#phoneNumberError").text("Phone numbers (numbers only) must be between 8 and 15 characters.");
            errorsFound = true;
        }

        if (errorsFound === true) {
            return;
        }

        this.props.onContactSubmit({ firstName: firstName, lastName: lastName, nickname: nickname, email: email, phoneNumbers: [phoneNumber] });
        this.setState({ firstName: '', lastName: '', nickname: '', email: '', phoneNumber: '' });
    },

    render: function () {
        return (
            <form className="contactForm" onSubmit={this.handleSubmit}>
                <div className="form-group">
                    <label for="firstNameInput">First Name *</label>
                    <input className="form-control" type="text" id="firstNameInput"
                           placeholder="First name"
                           value={this.state.firstName}
                           onChange={this.handleFirstNameChange} />
                    <span id="firstNameError" className="text-warning"></span>
                </div>

                <div className="form-group">
                    <label for="lastNameInput">Last Name</label>
                    <input className="form-control" type="text" id="lastNameInput"
                           placeholder="Last name"
                           value={this.state.lastName}
                           onChange={this.handleLastNameChange} />
                </div>

                <div className="form-group">
                    <label for="nicknameInput">Nickname</label>
                    <input className="form-control" type="text" id="nicknameInput"
                           placeholder="Nickname"
                           value={this.state.nickname}
                           onChange={this.handleNicknameChange} />
                </div>

                <div className="form-group">
                    <label for="emailInput">Email Address</label>
                    <input className="form-control" type="email" id="emailInput"
                           placeholder="Email Address"
                           value={this.state.email}
                           onChange={this.handleEmailChange} />
                </div>

                <div className="form-group">
                    <label for="phoneNumberInput">PhoneNumber</label>
                    <input className="form-control" type="tel" id="phoneNumberInput"
                           placeholder="e.g. 555-867-5309"
                           value={this.state.phoneNumber}
                           onChange={this.handlePhoneNumberChange} />
                    <span id="phoneNumberError" className="text-warning"></span>
                </div>

                <input type="submit" value="Create Contact" className="btn btn-primary" />
            </form>
        );
    }
});

ReactDOM.render(
    <ContactBox url="api/Contacts" pollInterval={2000} />,
    document.getElementById('content')
);