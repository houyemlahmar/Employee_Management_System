SELECT * FROM LeaveApplications
WHERE EmployeeId NOT IN (SELECT Id FROM Employes)
