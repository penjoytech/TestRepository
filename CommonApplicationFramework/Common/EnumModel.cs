namespace CommonApplicationFramework.Common
{
    public enum CompanyName
    {
        DemoMFL, DemoNewMFL, Pantaloons
    }

    public enum FielddataStatic
    {
        LeaseExpiryDate, CarpetArea, SBA, BusinessModel, Brand, SubBrand, NatureOfAgreement, AssignabilityOfContract, ConditionOfAssignment, IsSubLease, Attornment, DisputeResolution, LesseNoticePeriod, CarpetAreaUnit, SBAUnit, RentModel, Remarks
    }

    public enum PaymentModeEnum
    {
        DD, Cheque, RTGS, NEFT
    }

    public enum DCType
    {
        Security_Deposit, REG
    }

    public enum StatusType
    {
        Open, Close, WIP, OPEN, EXECUTED
    }

    public enum Location
    {
        Country, Region, State
    }

    public enum EventCode
    {
        EVE_REGEXP = 1, EVE_TERM = 2, EVE_CLOSURE = 3, EVE_RENEWAL = 4, ClosureDepositCharge, Notification, NotificationLog, EVE_SD = 15, EVE_CUST
    }

    public enum UsersStatus
    {
        Active, Inactive, Blocked, Unblocked, Deleted, Expired
    }

    public enum ResourceType
    {
        Module = 1, Page = 2, Panel = 3, Button = 4
    }

    public enum GroupTypes
    {
        DataAccessGroup = 1, UserManagementGroup = 2, NotificationGroup = 3, ReportGroup = 4, Standard =6, Privileged =7, Management=8
    }

    public enum SectionCode
    {
        SC_WIPStage, SC_WIP
    }

    public enum FilterSectionType
    {
        Leasing, WIP
    }

    public enum ObjectName
    {
        WIP_Region, WIP_state, WIP_StoreType, WIP_WIPStatus
    }

    public enum PropertyTypeEnum
    {
        Prop_Retail
    }

    public enum WIPStatusEnum
    {
        OPEN, ON_HOLD
    }

    public enum WIPstatuswithIdEnum
    {
        WIP_004 = 6
    }

    public enum Search
    {
        Dropdown, TextBox, In, BETWEEN, Matches
    }

    public enum AgreementstatusEnum
    {
        ExecutedAndNotRegistered, ExecutedAndRegistered, EXE_REG, EXE_NOTREG, EXE_CANNOTREG
    }

    public enum StoreTypeEnum
    {
        High_Street, Mall
    }

    public enum RentTypeEnum
    {
        Fixed_Rent, Revenue_Sharing, Rent_Amenities
    }

    public enum DepositChargeEnum
    {
        Security_Deposit, CAM, Electricity_Bills, Other_Charges, Stamp_Duty, Registration_Fee, Water_Bill, Electricity_Deposit, Rent, Amenities
    }

    public enum NotificationTimeLine
    {
        Before, After
    }

    public enum ActivityLog
    {
        LOGIN = 1, LOGOUT = 2, ADD_USER = 3, CHANGE_PASSWORD = 4, FORGOT_PASSWORD = 5, UPDATE_USER = 6, ADD_GROUP = 7
    }

    public enum LicenseEnumCode
    {
        Created, Validate, Expired, Activated, Trial, Enterprise
    }

    public enum ResponseType
    {
        Success, Failure, NoContent, NotFound, Created, NotCreated, Modified, NotModified, Deleted, NotDeleted, InvalidRequest, Duplicate
    }

    public enum ActionConstraint
    {
        CAN_VIEW_ALL_LANDLORD, CAN_VIEW_LANDLORD, CAN_SEARCH_LANDLORD, CAN_ADD_LANDLORD, CAN_LINK_LANDLORD, CAN_EDIT_LANDLORD, CAN_LINK_DELINK_LANDLORD, CAN_DELETE_LANDLORD, CAN_DELETE_LANDLORDPROP,

        CAN_VIEW_LEASE, CAN_ADD_LEASE, CAN_EDIT_LEASEBASE, CAN_EDIT_IMAGE, CAN_EDIT_LEASE_SUPPLEMENTARY,

        CAN_EDIT_LEASE, CAN_SEARCH_LEASE, CAN_ADD_LEASE_VERSION,

        CAN_VIEW_TERMINATION, CAN_ADD_TERMINATION, CAN_EDIT_TERMINATION, CAN_DEACTIVATE_TERMINATION, CAN_CLOSE_TERMINATION, CAN_DELETE_TERMINATION,

        CAN_VIEW_DC_PAYMENTS, CAN_VIEW_PAYMENTMODES, CAN_VIEW_DEPOSITCHARGES, CAN_ADD_DC, CAN_ADD_DC_PAYMENT, CAN_EDIT_DC, CAN_EDIT_DC_PAYMENT, CAN_DELETE_DC, CAN_DELETE_DC_PAYMENT,

        CAN_ADD_CONSIDERATION, CAN_EDIT_CONSIDERATION, CAN_DELETE_CONSIDERATION,

        CAN_VIEW_LOCATION_MASTER, CAN_VIEW_LEASING_MASTER, CAN_VIEW_WIP_STAGE, CAN_VIEW_WIP_STAKE_HOLDERS, CAN_VIEW_DOCUMENT_TYPE, CAN_VIEW_SECTION_MASTER,

        CAN_VIEW_AGREEMENT_EXECUTED_DASHBOARD, CAN_VIEW_ENR_DASHBOARD, CAN_VIEW_EANR_DASHBOARD, CAN_VIEW_STORES_DASHBOARD, CAN_VIEW_FYSTORES_DASHBOARD, CAN_VIEW_STORE_STATUS_DASHBOARD, CAN_VIEW_LOCATION_WISE_STORES_DASHBOARD, CAN_VIEW_REGISTERED_STORES_COUNT_DASHBOARD, CAN_VIEW_CLOSERSTORESCOUNT_DASHBOARD,

        CAN_VIEW_ANNOUNCEMENT, CAN_ADD_ANNOUNCEMENT, CAN_EDIT_ANNOUNCEMENT, CAN_DELETE_ANNOUNCEMENT,

        CAN_ADD_LEASE_FOLDER, CAN_DELETE_LEASE_FOLDER, CAN_ADD_LEASE_GROUP_FOLDER, CAN_DELETE_LEASE_GROUP_FOLDER,

        CAN_ADD_NEWS, CAN_EDIT_NEWS, CAN_DELETE_NEWS,

        CAN_ADD_NEWSLETTER, CAN_EDIT_NEWSLETTER, CAN_DELETE_NEWSLETTER,

        CAN_ADD_MEDIA,

        CAN_ADD_HUMOR, CAN_EDIT_HUMOR, CAN_DELETE_HUMOR,

        CAN_ADD_LEGAL_TEAM, CAN_EDIT_LEGAL_TEAM, CAN_DELETE_LEGAL_TEAM,

        CAN_VIEW_ALL_WIP, CAN_VIEW_WIP, CAN_ADD_WIP, CAN_EDIT_WIP, CAN_DELETE_WIP,

        CAN_ADD_WIP_STAGE, CAN_UPDATE_WIP_STAGE, CAN_DELETE_WIP_STAGE,

        CAN_ADD_WIP_STAKE_HOLDER, CAN_UPDATE_WIP_STAKE_HOLDER, CAN_DELETE_WIP_STAKE_HOLDER,

        CAN_VIEW_LEGAL_NOTE, CAN_ADD_LEGAL_NOTE,

        CAN_VIEW_FOLDER, CAN_ADD_FOLDER, CAN_EDIT_FOLDER,

        CAN_VIEW_MODULE
    }

    public enum LoginType
    {
        User, Customer
    }

    public enum LocationCode
    {
        TERMINATION, CONSIDERATION, WIP, LEASE, WIPSTAGE
    }

    public enum MetaDataEnum
    {
        LANO, STORECODE, IMAGETYPE
    }
    public enum EmailEngine
    {
        Google,AWS
    }
}
