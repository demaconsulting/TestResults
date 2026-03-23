# Summary

25 of 38 requirements are satisfied with tests.

# Requirements

## TestOutcome Unit Requirements

| ID | Tests Linked | Passed | Failed | Not Executed |
| :- | -----------: | :-: | :-: | :-: |
| TestResults-Mdl-PassedOutcome | 1 | 1 | 0 | 0 |
| TestResults-Mdl-FailedOutcome | 1 | 1 | 0 | 0 |
| TestResults-Mdl-ErrorOutcome | 3 | 3 | 0 | 0 |
| TestResults-Mdl-TimeoutOutcome | 2 | 2 | 0 | 0 |
| TestResults-Mdl-NotExecutedOutcome | 4 | 4 | 0 | 0 |
| TestResults-Mdl-InconclusiveOutcome | 2 | 2 | 0 | 0 |
| TestResults-Mdl-AbortedOutcome | 1 | 1 | 0 | 0 |
| TestResults-Mdl-PendingOutcome | 3 | 3 | 0 | 0 |
| TestResults-Mdl-WarningOutcome | 1 | 1 | 0 | 0 |
| TestResults-Mdl-PassedButRunAbortedOutcome | 2 | 2 | 0 | 0 |
| TestResults-Mdl-NotRunnableOutcome | 1 | 1 | 0 | 0 |
| TestResults-Mdl-CompletedOutcome | 1 | 1 | 0 | 0 |
| TestResults-Mdl-InProgressOutcome | 1 | 1 | 0 | 0 |
| TestResults-Mdl-DisconnectedOutcome | 1 | 1 | 0 | 0 |

## TestResult Unit Requirements

| ID | Tests Linked | Passed | Failed | Not Executed |
| :- | -----------: | :-: | :-: | :-: |
| TestResults-Mdl-TestOutput | 4 | 4 | 0 | 0 |
| TestResults-Mdl-ErrorInfo | 4 | 4 | 0 | 0 |

## TestResults Unit Requirements

| ID | Tests Linked | Passed | Failed | Not Executed |
| :- | -----------: | :-: | :-: | :-: |
| TestResults-Mdl-Collection | 6 | 6 | 0 | 0 |

## Serializer Unit Requirements

| ID | Tests Linked | Passed | Failed | Not Executed |
| :- | -----------: | :-: | :-: | :-: |
| TestResults-Ser-FormatIdentify | 3 | 3 | 0 | 0 |
| TestResults-Ser-FormatConversion | 4 | 4 | 0 | 0 |
| TestResults-Ser-RoundTrip | 4 | 4 | 0 | 0 |

## TrxSerializer Unit Requirements

| ID | Tests Linked | Passed | Failed | Not Executed |
| :- | -----------: | :-: | :-: | :-: |
| TestResults-Trx-Serialize | 2 | 2 | 0 | 0 |
| TestResults-Trx-Deserialize | 4 | 4 | 0 | 0 |

## JUnitSerializer Unit Requirements

| ID | Tests Linked | Passed | Failed | Not Executed |
| :- | -----------: | :-: | :-: | :-: |
| TestResults-Jun-Serialize | 6 | 6 | 0 | 0 |
| TestResults-Jun-Deserialize | 6 | 6 | 0 | 0 |

## TestResults Library Requirements

### Runtime

| ID | Tests Linked | Passed | Failed | Not Executed |
| :- | -----------: | :-: | :-: | :-: |
| TestResults-Run-Net8 | 2 | 0 | 0 | 2 |
| TestResults-Run-Net9 | 2 | 0 | 0 | 2 |
| TestResults-Run-NetStd20 | 2 | 0 | 0 | 2 |
| TestResults-Run-Net10 | 2 | 0 | 0 | 2 |

## Platform Support

| ID | Tests Linked | Passed | Failed | Not Executed |
| :- | -----------: | :-: | :-: | :-: |
| TestResults-Platform-Windows | 1 | 0 | 0 | 1 |
| TestResults-Platform-Linux | 1 | 0 | 0 | 1 |
| TestResults-Platform-MacOS | 1 | 0 | 0 | 1 |

## OTS Software Requirements

### MSTest Requirements

| ID | Tests Linked | Passed | Failed | Not Executed |
| :- | -----------: | :-: | :-: | :-: |
| TestResults-OTS-MSTest | 4 | 4 | 0 | 0 |

### ReqStream Requirements

| ID | Tests Linked | Passed | Failed | Not Executed |
| :- | -----------: | :-: | :-: | :-: |
| TestResults-OTS-ReqStream | 1 | 0 | 0 | 1 |

### BuildMark Requirements

| ID | Tests Linked | Passed | Failed | Not Executed |
| :- | -----------: | :-: | :-: | :-: |
| TestResults-OTS-BuildMark | 1 | 0 | 0 | 1 |

### VersionMark Requirements

| ID | Tests Linked | Passed | Failed | Not Executed |
| :- | -----------: | :-: | :-: | :-: |
| TestResults-OTS-VersionMark | 2 | 0 | 0 | 2 |

### SarifMark Requirements

| ID | Tests Linked | Passed | Failed | Not Executed |
| :- | -----------: | :-: | :-: | :-: |
| TestResults-OTS-SarifMark | 2 | 0 | 0 | 2 |

### SonarMark Requirements

| ID | Tests Linked | Passed | Failed | Not Executed |
| :- | -----------: | :-: | :-: | :-: |
| TestResults-OTS-SonarMark | 4 | 0 | 0 | 4 |

### ReviewMark Requirements

| ID | Tests Linked | Passed | Failed | Not Executed |
| :- | -----------: | :-: | :-: | :-: |
| TestResults-OTS-ReviewMark | 2 | 0 | 0 | 2 |

# Testing

| Test | Requirement | Passed | Failed |
|------|-------------|--------|--------|
| BuildMark_MarkdownReportGeneration | TestResults-OTS-BuildMark | 0 | 0 |
| JUnitSerializer_Deserialize_BasicJUnitXml_ReturnsTestResults | TestResults-Jun-Deserialize | 1 | 0 |
| JUnitSerializer_Deserialize_BasicJUnitXml_ReturnsTestResults | TestResults-OTS-MSTest | 1 | 0 |
| JUnitSerializer_Deserialize_BasicJUnitXml_ReturnsTestResults | TestResults-Ser-RoundTrip | 1 | 0 |
| JUnitSerializer_Deserialize_ErrorTest_ReturnsErrorDetails | TestResults-Jun-Deserialize | 1 | 0 |
| JUnitSerializer_Deserialize_ErrorTest_ReturnsErrorDetails | TestResults-Mdl-ErrorOutcome | 1 | 0 |
| JUnitSerializer_Deserialize_FailedTest_ReturnsFailureDetails | TestResults-Jun-Deserialize | 1 | 0 |
| JUnitSerializer_Deserialize_FailedTest_ReturnsFailureDetails | TestResults-Mdl-ErrorInfo | 1 | 0 |
| JUnitSerializer_Deserialize_MultipleTestSuites_ReturnsAllTests | TestResults-Jun-Deserialize | 1 | 0 |
| JUnitSerializer_Deserialize_SkippedTest_ReturnsSkippedStatus | TestResults-Jun-Deserialize | 1 | 0 |
| JUnitSerializer_Deserialize_SkippedTest_ReturnsSkippedStatus | TestResults-Mdl-NotExecutedOutcome | 1 | 0 |
| JUnitSerializer_Deserialize_TestWithOutput_ReturnsSystemOutput | TestResults-Jun-Deserialize | 1 | 0 |
| JUnitSerializer_Deserialize_TestWithOutput_ReturnsSystemOutput | TestResults-Mdl-TestOutput | 1 | 0 |
| JUnitSerializer_Serialize_ErrorTest_IncludesErrorElement | TestResults-Jun-Serialize | 1 | 0 |
| JUnitSerializer_Serialize_ErrorTest_IncludesErrorElement | TestResults-Mdl-ErrorOutcome | 1 | 0 |
| JUnitSerializer_Serialize_FailedTest_IncludesFailureElement | TestResults-Jun-Serialize | 1 | 0 |
| JUnitSerializer_Serialize_FailedTest_IncludesFailureElement | TestResults-Mdl-ErrorInfo | 1 | 0 |
| JUnitSerializer_Serialize_MultipleTestsInClasses_GroupsByClassName | TestResults-Jun-Serialize | 1 | 0 |
| JUnitSerializer_Serialize_PassedTest_ProducesValidJUnitXml | TestResults-Jun-Serialize | 1 | 0 |
| JUnitSerializer_Serialize_PassedTest_ProducesValidJUnitXml | TestResults-OTS-MSTest | 1 | 0 |
| JUnitSerializer_Serialize_SkippedTest_IncludesSkippedElement | TestResults-Jun-Serialize | 1 | 0 |
| JUnitSerializer_Serialize_SkippedTest_IncludesSkippedElement | TestResults-Mdl-NotExecutedOutcome | 1 | 0 |
| JUnitSerializer_Serialize_TestWithOutput_IncludesSystemOutAndErr | TestResults-Jun-Serialize | 1 | 0 |
| JUnitSerializer_Serialize_TestWithOutput_IncludesSystemOutAndErr | TestResults-Mdl-TestOutput | 1 | 0 |
| JUnitSerializer_Serialize_ThenDeserialize_PreservesTestData | TestResults-Ser-FormatConversion | 1 | 0 |
| JUnitSerializer_Serialize_ThenDeserialize_PreservesTestData | TestResults-Ser-RoundTrip | 1 | 0 |
| macos@TrxSerializer_Serialize_BasicTestResults_ProducesValidTrxXml | TestResults-Platform-MacOS | 0 | 0 |
| net10.0@JUnitSerializer_Serialize_PassedTest_ProducesValidJUnitXml | TestResults-Run-Net10 | 0 | 0 |
| net10.0@TrxSerializer_Serialize_BasicTestResults_ProducesValidTrxXml | TestResults-Run-Net10 | 0 | 0 |
| net481@JUnitSerializer_Serialize_PassedTest_ProducesValidJUnitXml | TestResults-Run-NetStd20 | 0 | 0 |
| net481@TrxSerializer_Serialize_BasicTestResults_ProducesValidTrxXml | TestResults-Run-NetStd20 | 0 | 0 |
| net8.0@JUnitSerializer_Serialize_PassedTest_ProducesValidJUnitXml | TestResults-Run-Net8 | 0 | 0 |
| net8.0@TrxSerializer_Serialize_BasicTestResults_ProducesValidTrxXml | TestResults-Run-Net8 | 0 | 0 |
| net9.0@JUnitSerializer_Serialize_PassedTest_ProducesValidJUnitXml | TestResults-Run-Net9 | 0 | 0 |
| net9.0@TrxSerializer_Serialize_BasicTestResults_ProducesValidTrxXml | TestResults-Run-Net9 | 0 | 0 |
| ReqStream_EnforcementMode | TestResults-OTS-ReqStream | 0 | 0 |
| ReviewMark_ReviewPlanGeneration | TestResults-OTS-ReviewMark | 0 | 0 |
| ReviewMark_ReviewReportGeneration | TestResults-OTS-ReviewMark | 0 | 0 |
| SarifMark_MarkdownReportGeneration | TestResults-OTS-SarifMark | 0 | 0 |
| SarifMark_SarifReading | TestResults-OTS-SarifMark | 0 | 0 |
| Serializer_Deserialize_JUnitContent_ReturnsTestResults | TestResults-Ser-FormatConversion | 1 | 0 |
| Serializer_Deserialize_JUnitWithSystemOutput_ParsesCorrectly | TestResults-Mdl-TestOutput | 1 | 0 |
| Serializer_Deserialize_TrxContent_ReturnsTestResults | TestResults-Ser-FormatConversion | 1 | 0 |
| Serializer_Identify_JUnitTestsuiteContent_ReturnsJUnit | TestResults-Ser-FormatIdentify | 1 | 0 |
| Serializer_Identify_JUnitTestsuitesContent_ReturnsJUnit | TestResults-Ser-FormatIdentify | 1 | 0 |
| Serializer_Identify_TrxContent_ReturnsTrx | TestResults-Ser-FormatIdentify | 1 | 0 |
| Serializer_TrxSerializedResults_RoundTripsViaJUnit_PreservesCoreTestData | TestResults-Ser-FormatConversion | 1 | 0 |
| SonarMark_HotSpotsRetrieval | TestResults-OTS-SonarMark | 0 | 0 |
| SonarMark_IssuesRetrieval | TestResults-OTS-SonarMark | 0 | 0 |
| SonarMark_MarkdownReportGeneration | TestResults-OTS-SonarMark | 0 | 0 |
| SonarMark_QualityGateRetrieval | TestResults-OTS-SonarMark | 0 | 0 |
| TestOutcome_IsExecuted_AllOutcomes_ReturnsExpectedResult | TestResults-Mdl-CompletedOutcome | 1 | 0 |
| TestOutcome_IsExecuted_AllOutcomes_ReturnsExpectedResult | TestResults-Mdl-DisconnectedOutcome | 1 | 0 |
| TestOutcome_IsExecuted_AllOutcomes_ReturnsExpectedResult | TestResults-Mdl-InconclusiveOutcome | 1 | 0 |
| TestOutcome_IsExecuted_AllOutcomes_ReturnsExpectedResult | TestResults-Mdl-InProgressOutcome | 1 | 0 |
| TestOutcome_IsExecuted_AllOutcomes_ReturnsExpectedResult | TestResults-Mdl-NotRunnableOutcome | 1 | 0 |
| TestOutcome_IsExecuted_AllOutcomes_ReturnsExpectedResult | TestResults-Mdl-PassedButRunAbortedOutcome | 1 | 0 |
| TestOutcome_IsExecuted_AllOutcomes_ReturnsExpectedResult | TestResults-Mdl-PendingOutcome | 1 | 0 |
| TestOutcome_IsExecuted_NotExecutedOutcome_ReturnsFalse | TestResults-Mdl-NotExecutedOutcome | 1 | 0 |
| TestOutcome_IsFailed_FailedOutcome_ReturnsTrue | TestResults-Mdl-AbortedOutcome | 1 | 0 |
| TestOutcome_IsFailed_FailedOutcome_ReturnsTrue | TestResults-Mdl-ErrorOutcome | 1 | 0 |
| TestOutcome_IsFailed_FailedOutcome_ReturnsTrue | TestResults-Mdl-FailedOutcome | 1 | 0 |
| TestOutcome_IsFailed_FailedOutcome_ReturnsTrue | TestResults-Mdl-TimeoutOutcome | 1 | 0 |
| TestOutcome_IsPassed_PassedOutcome_ReturnsTrue | TestResults-Mdl-PassedButRunAbortedOutcome | 1 | 0 |
| TestOutcome_IsPassed_PassedOutcome_ReturnsTrue | TestResults-Mdl-PassedOutcome | 1 | 0 |
| TestOutcome_IsPassed_PassedOutcome_ReturnsTrue | TestResults-Mdl-PendingOutcome | 1 | 0 |
| TestOutcome_IsPassed_PassedOutcome_ReturnsTrue | TestResults-Mdl-WarningOutcome | 1 | 0 |
| TestResults_Id_Default_IsNotEmpty | TestResults-Mdl-Collection | 1 | 0 |
| TestResults_Id_TwoInstances_AreUnique | TestResults-Mdl-Collection | 1 | 0 |
| TestResults_Name_Default_IsEmpty | TestResults-Mdl-Collection | 1 | 0 |
| TestResults_Results_Default_IsEmpty | TestResults-Mdl-Collection | 1 | 0 |
| TestResults_Results_Default_IsNotNull | TestResults-Mdl-Collection | 1 | 0 |
| TestResults_UserName_Default_IsEmpty | TestResults-Mdl-Collection | 1 | 0 |
| TrxExampleTests_Deserialize_Example1Trx_ReturnsAllTestResults | TestResults-Mdl-InconclusiveOutcome | 1 | 0 |
| TrxExampleTests_Deserialize_Example1Trx_ReturnsAllTestResults | TestResults-Mdl-NotExecutedOutcome | 1 | 0 |
| TrxExampleTests_Deserialize_Example1Trx_ReturnsAllTestResults | TestResults-Mdl-PendingOutcome | 1 | 0 |
| TrxExampleTests_Deserialize_Example1Trx_ReturnsAllTestResults | TestResults-Mdl-TimeoutOutcome | 1 | 0 |
| TrxExampleTests_Deserialize_Example1Trx_ReturnsAllTestResults | TestResults-Trx-Deserialize | 1 | 0 |
| TrxExampleTests_Deserialize_Example2Trx_ReturnsAllTestResults | TestResults-Trx-Deserialize | 1 | 0 |
| TrxSerializer_Deserialize_BasicTrxXml_ReturnsTestResults | TestResults-OTS-MSTest | 1 | 0 |
| TrxSerializer_Deserialize_BasicTrxXml_ReturnsTestResults | TestResults-Ser-RoundTrip | 1 | 0 |
| TrxSerializer_Deserialize_BasicTrxXml_ReturnsTestResults | TestResults-Trx-Deserialize | 1 | 0 |
| TrxSerializer_Deserialize_ComplexTrxXml_ReturnsTestResults | TestResults-Mdl-ErrorInfo | 1 | 0 |
| TrxSerializer_Deserialize_ComplexTrxXml_ReturnsTestResults | TestResults-Trx-Deserialize | 1 | 0 |
| TrxSerializer_Serialize_BasicTestResults_ProducesValidTrxXml | TestResults-Mdl-TestOutput | 1 | 0 |
| TrxSerializer_Serialize_BasicTestResults_ProducesValidTrxXml | TestResults-OTS-MSTest | 1 | 0 |
| TrxSerializer_Serialize_BasicTestResults_ProducesValidTrxXml | TestResults-Trx-Serialize | 1 | 0 |
| TrxSerializer_Serialize_MultipleTestResults_ProducesValidTrxXml | TestResults-Trx-Serialize | 1 | 0 |
| TrxSerializer_Serialize_StackTraceWithoutMessage_IncludesStackTraceElement | TestResults-Mdl-ErrorInfo | 1 | 0 |
| TrxSerializer_Serialize_ThenDeserialize_PreservesTestData | TestResults-Ser-RoundTrip | 1 | 0 |
| ubuntu@TrxSerializer_Serialize_BasicTestResults_ProducesValidTrxXml | TestResults-Platform-Linux | 0 | 0 |
| VersionMark_CapturesVersions | TestResults-OTS-VersionMark | 0 | 0 |
| VersionMark_GeneratesMarkdownReport | TestResults-OTS-VersionMark | 0 | 0 |
| windows@TrxSerializer_Serialize_BasicTestResults_ProducesValidTrxXml | TestResults-Platform-Windows | 0 | 0 |

