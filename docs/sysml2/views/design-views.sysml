// Views rendered into docs/design/generated/ for the Design document.
// Reopens the TestResultsLibrary package so each 'expose' statement can reference
// its target part def directly. Per SysML2Tools syntax, 'expose' is only valid
// inside a named 'view' usage (not a 'view def' definition), and it - not
// 'render' - is what scopes the rendered diagram content.
package TestResultsLibrary {
    view SoftwareStructureView {
        expose TestResultsLibrary;
    }

    view TestResultsLibraryView {
        expose TestResultsLibrarySystem;
    }

    view IOView {
        expose IOSubsystem;
    }
}
