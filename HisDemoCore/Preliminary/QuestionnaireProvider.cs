using System;
using System.Collections.Generic;
using HisDemo.DTO;

namespace HisDemo.Preliminary
{
    /// <summary>
    /// This class implements hard-coded questionnaire configuration.
    /// Alternatively this could be replaced or extended to dynamically determine the questions/fields.
    /// </summary>
    public static class QuestionnaireProvider
    {
        public static List<Questionnaire> GetQuestionnaires(bool disable_consent = false)
        {
            List<Questionnaire> list = new List<Questionnaire>();

            {
                Questionnaire q = new Questionnaire();
                q.Title = "Persönliche Daten I";
                q.ShowInPrint = true;

                q.Questions.Add(new Question()
                {
                    QID = "NameGiven",
                    QuestionText = "Vorname",
                    Maximum = 63,
                    Required = true
                });

                q.Questions.Add(new Question()
                {
                    QID = "NameFamily",
                    QuestionText = "Nachname",
                    Maximum = 63,
                    Required = true
                });

                q.Questions.Add(new Question()
                {
                    QID = "BirthDate",
                    QuestionText = "Geburtsdatum",
                    QuestionType = QuestionType.Date,
                    Required = true
                });

                q.Questions.Add(new Question()
                {
                    QID = "ContactEmail",
                    QuestionText = "E-Mail Adresse",
                    QuestionType = QuestionType.EmailAddress,
                    Maximum = 127,
                    Required = true
                });

                q.Questions.Add(new Question()
                {
                    QID = "ContactPhone",
                    QuestionText = "Telefonnummer (möglichst Mobil)",
                    QuestionType = QuestionType.PhoneNumber,
                    Maximum = 63,
                    Required = true
                });

                q.Questions.Add(new Question()
                {
                    QID = "PrimaryLocationStreet",
                    QuestionText = "Primärer Aufenthaltsort Straße/Hausnr.",
                    Maximum = 255,
                    Required = true
                });

                q.Questions.Add(new Question()
                {
                    QID = "PrimaryLocationZIPCode",
                    QuestionText = "Primärer Aufenthaltsort PLZ",
                    QuestionType = QuestionType.PLZ,
                    Required = true
                });

                q.Questions.Add(new Question()
                {
                    QID = "PrimaryLocationLocality",
                    QuestionText = "Primärer Aufenthaltsort Ort",
                    Maximum = 255,
                    Required = true
                });

                q.Questions.Add(new Question()
                {
                    QID = "PersonalAddressStreet",
                    QuestionText = "Wohnsitz Straße/Hausnr.",
                    Maximum = 255,
                    Required = true
                });

                q.Questions.Add(new Question()
                {
                    QID = "PersonalAddressZIPCode",
                    QuestionText = "Wohnsitz PLZ",
                    QuestionType = QuestionType.PLZ,
                    Required = true
                });

                q.Questions.Add(new Question()
                {
                    QID = "PersonalAddressLocality",
                    QuestionText = "Wohnsitz Ort",
                    Maximum = 255,
                    Required = true
                });
                list.Add(q);
            }
            {
                Questionnaire q = new Questionnaire();
                q.Title = "Persönliche Daten II";
                q.ShowInPrint = true;

                q.Questions.Add(new Question()
                {
                    QID = "DoctorName",
                    QuestionText = "Haus-/Betriebsarzt",
                    QuestionType = QuestionType.Plaintext,
                    Maximum = 127,
                    Required = true,
                });
                q.Questions.Add(new Question()
                {
                    QID = "DoctorContact",
                    QuestionText = "Anschrift/Kontakt des Haus-/Betriebsarzt",
                    ShortText = "Anschrift/Kontakt des Haus-/Betriebsarzt",
                    QuestionType = QuestionType.Plaintext,
                    Maximum = 127,
                    Required = false,
                });

                q.Questions.Add(new Question()
                {
                    QID = "Krankenkasse",
                    QuestionText = "Krankenkasse (Kürzel reicht)",
                    ShortText = "Krankenkasse",
                    QuestionType = QuestionType.Plaintext,
                    Maximum = 63,
                    Required = true,
                });
                q.Questions.Add(new Question()
                {
                    QID = "OccupationPartnerInsitution",
                    QuestionText = "Für welche Partner-Institution arbeiten Sie?",
                    Maximum = 127,
                    Required = true,
                });
                list.Add(q);
            }

            {
                Questionnaire q = new Questionnaire();
                q.Title = "Fragebogen: Allgemein";

                q.Questions.Add(new Question()
                {
                    QID = "QNGender",
                    QuestionText = "Geschlecht (m/w/d)",
                    ShortText = "Geschlecht",
                    QuestionType = QuestionType.Gender,
                    Required = false,
                });

                q.Questions.Add(new Question()
                {
                    QID = "QNLivingAlone",
                    QuestionText = "Wohnen Sie alleine?",
                    QuestionType = QuestionType.YesNo,
                    Required = false,
                });
                q.Questions.Add(new Question()
                {
                    QID = "QNPrivateCare",
                    QuestionText = "Pflegen oder unterstützen Sie privat andere alte oder chronisch kranke Personen?",
                    QuestionType = QuestionType.YesNo,
                    Required = false,
                });
                q.Questions.Add(new Question()
                {
                    QuestionText = "Wenn Sie in einem der folgenden Bereiche tätig sind: wo?",
                    QuestionType = QuestionType.JustText,
                });
                q.Questions.Add(new Question()
                {
                    QID = "QNOccupationMedical",
                    QuestionText = "Medizinischer Bereich",
                    Required = false,
                });
                q.Questions.Add(new Question()
                {
                    QID = "QNOccupationSocial",
                    QuestionText = "Gemeinschaftseinrichtung (Schule, Kita, Heim, Uni)",
                    ShortText = "Gemeinschaftseinrichtung",
                    Required = false,
                });
                list.Add(q);
            }

            {
                Questionnaire q = new Questionnaire();
                q.Title = "Fragebogen: Risiken und Symptome";
                q.Questions.Add(new Question()
                {
                    QuestionText = "Wir möchten Ihnen jetzt einige Fragen zu möglichen Infektionsrisiken und Symptomen einer COVID-19 Infektion stellen.",
                    QuestionType = QuestionType.JustText,
                });

                q.Questions.Add(new Question()
                {
                    QID = "QNConfirmedCaseContact",
                    QuestionText = "Hatten Sie engen Kontakt zu einem bestätigten Fall?",
                    QuestionType = QuestionType.YesNoUnknown,
                    Required = false,
                });
                q.Questions.Add(new Question()
                {
                    QuestionText = "Hatten Sie innerhalb der letzten 14 Tagen eines oder mehrere der folgenden Symptome? Wenn ja geben Sie jeweils an, seit wievielen Tagen.",
                    QuestionType = QuestionType.JustText,
                });
                Func<string, string, Question> lambdaSymptom = (string key, string name) => new Question()
                {
                    QID = $"QNSymptom{key}SinceDays",
                    QuestionText = name,
                    QuestionType = QuestionType.Number,
                    Minimum = 1,
                    Maximum = 14,
                    Required = false,
                };
                q.Questions.Add(lambdaSymptom("Fever", "Fieber"));
                q.Questions.Add(lambdaSymptom("Shivering", "Schüttelfrost"));
                q.Questions.Add(lambdaSymptom("Fatigue", "vermehrt Müdigkeit oder eine deutlich geringere Belastbarkeit"));
                q.Questions.Add(lambdaSymptom("LimbPain", "Gliederschmerzen"));
                q.Questions.Add(lambdaSymptom("Headache", "Kopfschmerzen"));
                q.Questions.Add(lambdaSymptom("SoreThroat", "Halsschmerzen"));
                q.Questions.Add(lambdaSymptom("TasteAndSmellLoss", "Geschmacks- und Geruchsverlust"));

                list.Add(q);
            }

            if (true)
            {
                Questionnaire q = new Questionnaire();
                q.Title = "Fragebogen: Chronische Erkrankungen";

                q.Questions.Add(new Question()
                {
                    QuestionText = "Wurden bei Ihnen durch eine(n) Arzt/Ärztin folgende chronische Erkrankungen festgestellt?",
                    QuestionType = QuestionType.JustText,
                });

                Func<string, string, Question> lambdaChronisch = (string key, string name) => new Question()
                {
                    QID = $"QNChronicConfirmed{key}",
                    QuestionText = name,
                    QuestionType = QuestionType.YesNoUnknown,
                    Required = false,
                };
                q.Questions.Add(lambdaChronisch("Lung", "chronische Lungenerkrankung"));
                q.Questions.Add(lambdaChronisch("Diabetes", "Diabetes"));
                q.Questions.Add(lambdaChronisch("Heart", "Herzkrankheit"));
                q.Questions.Add(lambdaChronisch("Adiposity", "Adipositas (Fettsucht)"));
                q.Questions.Add(lambdaChronisch("Bowel", "chronische Darmerkrankung"));

                list.Add(q);
            }

            {
                Questionnaire q = new Questionnaire();
                q.Title = "Fragebogen: Weitere Symptome";

                q.Questions.Add(new Question()
                {
                    QuestionText = "Für die folgenden Fragen ist es wichtig, ob Sie an einer chronischen Erkrankung wie chronischer Bronchitis,"+
                    " Allergie, chronische Darmerkrankung leiden. Vergleichen Sie Ihre derzeitigen Beschwerden mit den bisherigen Problemen." +
                    " Hatten Sie in den letzten zwei Wochen:",
                    QuestionType = QuestionType.JustText,
                });

                Func<string, string, Question> lambdaRecent = (string key, string name) => new Question()
                {
                    QID = $"QNLast2Weeks{key}",
                    QuestionText = name,
                    QuestionType = QuestionType.YesNo,
                    Required = false,
                };
                q.Questions.Add(lambdaRecent("SustainedCough", "anhaltenden Husten"));
                q.Questions.Add(lambdaRecent("SustainedCold", "anhaltenden Schnupfen"));
                q.Questions.Add(lambdaRecent("Diarrhea", "Durchfall"));

                q.Questions.Add(new Question()
                {
                    QID = "QNLast2WeeksOutOfBreath",
                    QuestionText = "Sind Sie in den letzten zwei Wochen schneller außer Atem als sonst (Erklärung s.u.)?",
                    ShortText = "Letzte zwei Wochen schneller außer Atem",
                    QuestionType = QuestionType.YesNo,
                    Required = false,
                });
                q.Questions.Add(new Question()
                {
                    QuestionText = "Wählen Sie vorstehend ja, wenn Sie:\n * bei leichten Belastungen wie Spaziergang oder Treppensteigen schneller als sonst kurzatmig werden oder Schwierigkeiten beim Atmen haben" +
                    "\n * das Gefühl der Atemnot/Luftnot oder Kurzatmigkeit beim Sitzen oder Liegen verspühren" +
                    "\n * beim Aufstehen aus dem Bett oder vom Stuhl das Gefühl der Atemnot/Luftnot haben",
                    QuestionType = QuestionType.JustText,
                });

                list.Add(q);
            }

            {
                Questionnaire q = new Questionnaire();
                q.Title = "Fragebogen: Sonstiges";

                q.Questions.Add(new Question()
                {
                    QID = "QNPregnant",
                    QuestionText = "Sind sie schwanger?",
                    QuestionType = QuestionType.YesNoUnknown,
                    Required = false,
                });
                q.Questions.Add(new Question()
                {
                    QID = "QNSmoker",
                    QuestionText = "Rauchen sie?",
                    QuestionType = QuestionType.YesNo,
                    Required = false,
                });
                q.Questions.Add(new Question()
                {
                    QID = "QNTakingCortison",
                    QuestionText = "Nehmen Sie aktuell Cortison ein (in Tablettenform)?",
                    QuestionType = QuestionType.YesNoUnknown,
                    Required = false,
                });
                q.Questions.Add(new Question()
                {
                    QID = "QNTakingImmunosuppressives",
                    ShortText = "Nehmen Sie aktuell Immunsuppressiva?",
                    QuestionText = "Nehmen Sie aktuell Immunsuppressiva? (Immunsuppressiva nehmen oder bekommen Sie nach einer Organtransplantation, während der Therapie einer Autoimmunerkrankung oder im Rahmen einer Chemotherapie.)",
                    QuestionType = QuestionType.YesNoUnknown,
                    Required = false,
                });
                q.Questions.Add(new Question()
                {
                    QID = "QNFluShotSinceOctober2019",
                    QuestionText = "Haben Sie sich im Zeitraum Oktober 2019 bis heute gegen Grippe impfen lassen?",
                    QuestionType = QuestionType.YesNoUnknown,
                    Required = false,
                });

                q.Questions.Add(new Question()
                {
                    QuestionText = "Vielen Dank für Ihre Antworten!",
                    QuestionType = QuestionType.JustText,
                });
                list.Add(q);
            }

            {
                Questionnaire q = new Questionnaire();
                q.Title = "Einwilligung";

                q.ShowToPatient = true; // false;

                {
                    q.Questions.Add(new Question()
                    {
                        QID = "ConsentMCATest",
                        ShortText = "Einwilligung in den MCA-Test",
                        QuestionText = "Einwilligung in den MCA-Test (Rachenabstrich und Verfahren laut Informationsblatt. Entbindung von ärztlicher Schweigepflicht gegenüber Haus-/Betriebsarzt)",
                        QuestionType = QuestionType.YesNo,
                        Required = true,
                        MustAnswerYes = true,
                    });
                
                    q.Questions.Add(new Question()
                    {
                        QID = "ConsentPersonalData",
                        QuestionText = "Einwilligung zur Verarbeitung personenbezogener Daten",
                        QuestionType = QuestionType.YesNo,
                        Required = true,
                        MustAnswerYes = true,
                    });
                }

                {
                    q.Questions.Add(new Question()
                    {
                        QID = "ConsentSpecimenStorageAndUsage",
                        QuestionText = "Einwilligung zur Lagerung und Nutzung meiner Biomaterialien",
                        QuestionType = QuestionType.YesNo,
                        Required = true,
                    });
                    q.Questions.Add(new Question()
                    {
                        QuestionText = "Ich erkläre mich damit einverstanden, über folgende Kontaktdaten von Mitarbeiter*innen des MCA-Labors kontaktiert und zur Beteiligung an einer Online-Befragung eingeladen zu werden:",
                        QuestionType = QuestionType.JustText
                    });
                    q.Questions.Add(new Question()
                    {
                        QID = "ConsentExternalStudyEMail",
                        QuestionText = "per E-Mail",
                        QuestionType = QuestionType.YesNo,
                        Required = true,
                    });
                    q.Questions.Add(new Question()
                    {
                        QID = "ConsentExternalStudyPhone",
                        QuestionText = "Telefonisch",
                        QuestionType = QuestionType.YesNo,
                        Required = true,
                    });
                    q.Questions.Add(new Question()
                    {
                        QID = "ConsentExternalStudyWhatsApp",
                        QuestionText = "per WhatsApp",
                        QuestionType = QuestionType.YesNo,
                        Required = true,
                    });
                    q.Questions.Add(new Question()
                    {
                        QuestionText = "Ich erkläre mich damit einverstanden, über folgende Kontaktdaten von Mitarbeiter*innen des MCA-Labor kontaktiert zu werden, bezüglich zukünftiger Forschungsvorhaben/Studien:",
                        QuestionType = QuestionType.JustText
                    });
                    q.Questions.Add(new Question()
                    {
                        QID = "ConsentMCARecontactEMail",
                        QuestionText = "per E-Mail",
                        QuestionType = QuestionType.YesNo,
                        Required = true,
                    });
                    q.Questions.Add(new Question()
                    {
                        QID = "ConsentMCARecontactPhone",
                        QuestionText = "Telefonisch",
                        QuestionType = QuestionType.YesNo,
                        Required = true,
                    });
                }
                if (!disable_consent)
                {
                    list.Add(q);
                }
            }

            {
                Questionnaire q = new Questionnaire();
                q.Title = "Probenzuordnung";

                q.ShowToPatient = false;
                {
                    q.Questions.Add(new Question()
                    {
                        QID = "SpecimenIDTaken",
                        QuestionText = "ID-Nummer der Probe eingeben oder Scannen:",
                        QuestionType = QuestionType.B32IDOfSpecimen,
                        Required = true,
                    });
                }

                list.Add(q);
            }

            {
                // Validate
                List<string> keys = new List<string>();
                foreach (Questionnaire qn in list)
                {
                    foreach (Question q in qn.Questions)
                    {
                        if (q.QuestionType == QuestionType.JustText) continue;
                        string k = q.QID.ToLower().Trim();
                        if (keys.Contains(k))
                        {
                            throw new Exception($"Questionnaire duplicate QID! ({k})");
                        }
                        else
                        {
                            keys.Add(k);
                        }
                    }
                }
            }

            return list;
        }
    }
}
